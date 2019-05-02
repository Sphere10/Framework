//-----------------------------------------------------------------------
// <copyright file="ComponentRegistry.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Sphere10.Framework;

namespace Sphere10.Framework.Application {

    /// <summary>
    /// Sphere 10 one stop shop for all IoC and Service Locator functionality.
    /// <remarks>
    /// Under root configuration element add
    ///   &lt;configSections&gt;
    ///    &lt;section name = "ComponentRegistry" type ="Sphere10.Framework.GenericSectionHandler, Sphere10.Framework"/&gt;
    ///  &lt;/configSections&gt;
    /// 
    /// In Program.cs or Web startup, use
    ///  ...
    ///  ComponentRegistry.Instance.RegisterAppConfig();
    /// 
    /// if using application framework, this this is called automatically on start
    /// </remarks>
    /// </summary>
    [XmlRoot]
    public class ComponentRegistry : IDisposable {
        private readonly object _threadLock;
        private readonly TinyIoCContainer _tinyIoCContainer;
        private readonly IList<Registration> _registrations;
        private readonly LookupEx<Type, Registration> _registrationsByInterfaceType;
        private readonly LookupEx<Type, Registration> _registrationsByImplementationType;

        static ComponentRegistry() {
            Instance = new ComponentRegistry(TinyIoCContainer.Current);
        }

        private ComponentRegistry(TinyIoCContainer tinyIoCContainer) {
            _threadLock = new object();
            _tinyIoCContainer = tinyIoCContainer;
            _registrations = new List<Registration>();
            _registrationsByInterfaceType = new LookupEx<Type, Registration>();
            _registrationsByImplementationType = new LookupEx<Type, Registration>();
            State = 0;
        }

        private ComponentRegistry(ComponentRegistry parent) {
            _threadLock = new object();
            _tinyIoCContainer = parent._tinyIoCContainer.GetChildContainer();
            _registrations = new List<Registration>(parent._registrations);
            _registrationsByInterfaceType = new LookupEx<Type, Registration>(parent._registrationsByInterfaceType);
            _registrationsByImplementationType = new LookupEx<Type, Registration>(parent._registrationsByImplementationType);
            State = 0;
        }


        public static ComponentRegistry Instance { get; }
        
        public IEnumerable<Registration> Registrations => _registrations;

        public int State { get; private set; }

        #region Public Methods

        public ComponentRegistry GetChildRegistry() {
            return new ComponentRegistry(this);
        }

        public void RegisterDefinition(ComponentRegistryDefinition componentRegistryDefinition) {
            if (componentRegistryDefinition.RegistrationsDefinition == null)
                return;
            foreach (var registration in componentRegistryDefinition.RegistrationsDefinition) {
                TypeSwitch.Do(registration,
                    TypeSwitch.Case<ComponentRegistryDefinition.AssemblyRegistrationDefinition>(assemblyRegistration =>
                        RegisterAssemblyRegistration(
                            componentRegistryDefinition,
                            assemblyRegistration
                        )
                    ),
                    TypeSwitch.Case<ComponentRegistryDefinition.ComponentRegistrationDefinition>(componentRegistration =>
                        RegisterComponentRegistration(
                            componentRegistryDefinition,
                            componentRegistration
                        )
                    ),
                    TypeSwitch.Case<ComponentRegistryDefinition.ProxyInterfaceRegistrationDefinition>(proxyRegistration =>
                        RegisterProxyRegistration(
                            componentRegistryDefinition,
                            proxyRegistration
                        )
                    ),
                    TypeSwitch.Case<ComponentRegistryDefinition.ComponentSetRegistrationDefinition>(multipleComponentsRegistration =>
                        RegisterMultipleComponentsRegistration(
                            componentRegistryDefinition,
                            multipleComponentsRegistration
                        )
                    ),
                    TypeSwitch.Default(() => { throw new NotSupportedException(registration.GetType().FullName); })
                );
            }
        }

        public void RegisterComponentInstance<TInterface>(TInterface instance, string name = null)
            where TInterface : class {
            lock (_threadLock) {
                _tinyIoCContainer.Register(instance, name ?? string.Empty);
                Register(Registration.From(instance, name ?? string.Empty));
            }
        }

	    public void RegisterComponent<TInterface, TImplementation>(ActivationType activation) 
			where TInterface : class
			where TImplementation : class, TInterface {
			RegisterComponent<TInterface, TImplementation>(null, activation);
	    }

		public void RegisterComponent<TInterface, TImplementation>(string resolveKey = null, ActivationType activation = ActivationType.Instance)
            where TInterface : class
            where TImplementation : class, TInterface {
            lock (_threadLock) {
                var tinyOptions = _tinyIoCContainer.Register<TInterface, TImplementation>(resolveKey ?? string.Empty);
                switch (activation) {
                    case ActivationType.Instance:
                        tinyOptions.AsMultiInstance();
                        break;
                    case ActivationType.Singleton:
                        tinyOptions.AsSingleton();
                        break;
                    case ActivationType.PerRequest:
                        tinyOptions.AsPerRequestSingleton();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(activation), activation, null);
                }
                Register(Registration.From<TInterface, TImplementation>(resolveKey ?? string.Empty, activation));
            }
        }

        public void RegisterComponent(Type interfaceType, Type componentType, string resolveKey = null, ActivationType options = ActivationType.Instance) {
            lock (_threadLock) {
                var tinyOptions = _tinyIoCContainer.Register(interfaceType, componentType, resolveKey ?? string.Empty);
                switch (options) {
                    case ActivationType.Instance:
                        tinyOptions.AsMultiInstance();
                        break;
                    case ActivationType.Singleton:
                        tinyOptions.AsSingleton();
                        break;
                    case ActivationType.PerRequest:
                        tinyOptions.AsPerRequestSingleton();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options), options, null);
                }
                Register(Registration.From(interfaceType, componentType, resolveKey ?? string.Empty, options));
            }
        }

        public void RegisterProxyComponent<TInterface, TProxy>(string resolveKey = null) 
            where TInterface : class 
            where TProxy : class {
            lock (_threadLock) {
                _tinyIoCContainer.Register((container, overloads) => container.Resolve<TProxy>(overloads) as TInterface, resolveKey ?? string.Empty);
                Register(Registration.FromProxy<TInterface, TProxy>(resolveKey ?? string.Empty));
            }
        }

        public void RegisterProxyComponent(Type interfaceType, Type proxyType, string resolveKey = null) {
            lock (_threadLock) {
                if (!proxyType.IsAssignableFrom(proxyType))
                    throw new SoftwareException("Unable to register proxy component '{0}' as it is not a sub-type of '{1}'", proxyType.FullName, interfaceType.FullName);
                _tinyIoCContainer.Register(interfaceType, (container, overloads) => container.Resolve(proxyType, overloads), resolveKey ?? string.Empty);
                Register(Registration.FromProxy(interfaceType, proxyType, resolveKey ?? string.Empty));

            }
        }

        public TInterface Resolve<TInterface>(string resolveKey = null) where TInterface : class {
            var resolvedImplementation = _tinyIoCContainer.Resolve<TInterface>(resolveKey ?? string.Empty);
            //if (!TryResolve(out resolvedImplementation, name)) {
            //    throw new SoftwareException(
            //        "No component has been registered for service type '{0}' with name {1}",
            //        typeof (I).Name,
            //        string.IsNullOrEmpty(name) ? "(none)" : ("'" + name + "'")
            //        );
            //}
            return resolvedImplementation;
        }

        public object Resolve(Type type, string resolveKey = null)  {
            var resolvedImplementation = _tinyIoCContainer.Resolve(type, resolveKey ?? string.Empty);
            return resolvedImplementation;
        }

        public bool TryResolve<TInterface>(out TInterface component, string resolveKey = null) where TInterface : class {
            return _tinyIoCContainer.TryResolve(resolveKey ?? string.Empty, out component);
        }

        public IEnumerable<TInterface> ResolveAll<TInterface>() where TInterface : class {
            return _tinyIoCContainer.ResolveAll<TInterface>();
        }

        public IEnumerable<object> ResolveAll(Type type) {
            return _tinyIoCContainer.ResolveAll(type, true);
        }

        public bool HasImplementation<TImplementation>(string resolveKey = null) {
            return HasImplementation(typeof(TImplementation), resolveKey);
        }

        public bool HasImplementation(Type type, string resolveKey = null) {
            if (!string.IsNullOrWhiteSpace(resolveKey)) {
                return _registrationsByImplementationType[type].Any(r => r.ResolveKey == resolveKey);
            }
            return _registrationsByImplementationType.CountForKey(type) > 0;
        }

        public bool HasImplementationFor<TInterface>(string resolveKey = null) {
            return HasImplementationFor(typeof(TInterface), resolveKey);
        }

        public bool HasImplementationFor(Type type, string resolveKey = null) {
            if (!string.IsNullOrWhiteSpace(resolveKey)) {
                return _registrationsByInterfaceType[type].Any(r => r.ResolveKey == resolveKey);
            }
            return _registrationsByInterfaceType.CountForKey(type) > 0;
        }

        public void Dispose() {
            _tinyIoCContainer.Dispose();
            State = -1;
            this._registrations.Clear();
            this._registrationsByImplementationType.Clear();
            this._registrationsByInterfaceType.Clear();
        }

        #endregion

        #region Private Methods

        private void RegisterAssemblyRegistration(ComponentRegistryDefinition componentRegistryDefinition, ComponentRegistryDefinition.AssemblyRegistrationDefinition assemblyRegistrationDefinition) {
            TypeResolver.LoadAssembly(assemblyRegistrationDefinition.Dll, componentRegistryDefinition.PluginFolder);
        }

        private void RegisterComponentRegistration( ComponentRegistryDefinition componentRegistryDefinition, ComponentRegistryDefinition.ComponentRegistrationDefinition componentRegistrationDefinition) {
			if (string.IsNullOrWhiteSpace(componentRegistrationDefinition.Implementation))
				throw new ArgumentException("No implementation field provided in component registry definition");
            var interfaceType = TypeResolver.Resolve(componentRegistrationDefinition.Interface, assemblyHint: componentRegistrationDefinition.Dll);
            var implementationType = TypeResolver.Resolve(componentRegistrationDefinition.Implementation, componentRegistrationDefinition.Dll, componentRegistryDefinition.PluginFolder);
            var activation = componentRegistrationDefinition.Activation;

            RegisterComponent(
                interfaceType,
                implementationType,
                componentRegistrationDefinition.ResolveKey,
                activation ?? ActivationType.Instance
            );
        }

        private void RegisterProxyRegistration(ComponentRegistryDefinition componentRegistryDefinition, ComponentRegistryDefinition.ProxyInterfaceRegistrationDefinition proxyInterfaceRegistrationDefinition) {
            var interfaceType = TypeResolver.Resolve(proxyInterfaceRegistrationDefinition.Interface);
            var implementationType = TypeResolver.Resolve(proxyInterfaceRegistrationDefinition.Proxy);

            RegisterProxyComponent(
                interfaceType,
                implementationType
            );
        }

        private void RegisterMultipleComponentsRegistration(ComponentRegistryDefinition componentRegistryDefinition, ComponentRegistryDefinition.ComponentSetRegistrationDefinition componentSetRegistrationDefinition) {
            if (componentSetRegistrationDefinition.RegistrationsDefinition == null)
                return;

            foreach (var registration in componentSetRegistrationDefinition.RegistrationsDefinition) {
                TypeSwitch.Do(registration,
                    TypeSwitch.Case<ComponentRegistryDefinition.ComponentRegistrationDefinition>(componentRegistration => {
                        componentRegistration.Interface = componentSetRegistrationDefinition.Interface;
                        RegisterMutipleComponentComponentRegistration(
                            componentRegistryDefinition,
                            componentRegistration
                        );
                    }),
                    TypeSwitch.Case<ComponentRegistryDefinition.ProxyInterfaceRegistrationDefinition>(proxyRegistration => {
                        proxyRegistration.Interface = componentSetRegistrationDefinition.Interface;
                        RegisterMultipleComponentProxyRegistration(
                            componentRegistryDefinition,
                            proxyRegistration
                        );
                    }),
                    TypeSwitch.Default(() => { throw new NotSupportedException(registration.GetType().FullName); })
                );

            }
        }

        private void RegisterMutipleComponentComponentRegistration(ComponentRegistryDefinition componentRegistryDefinition, ComponentRegistryDefinition.ComponentRegistrationDefinition componentRegistrationDefinition) {
            var interfaceType = TypeResolver.Resolve(componentRegistrationDefinition.Interface, assemblyHint: componentRegistrationDefinition.Dll);
            var implementationType = TypeResolver.Resolve(componentRegistrationDefinition.Implementation, componentRegistrationDefinition.Dll, componentRegistryDefinition.PluginFolder);
            RegisterComponent(
                interfaceType,
                implementationType,
                componentRegistrationDefinition.ResolveKey ?? implementationType.FullName,
                componentRegistrationDefinition.Activation ?? ActivationType.Instance
            );
        }

        private void RegisterMultipleComponentProxyRegistration(ComponentRegistryDefinition componentRegistryDefinition, ComponentRegistryDefinition.ProxyInterfaceRegistrationDefinition proxyInterfaceRegistrationDefinition) {
            var interfaceType = TypeResolver.Resolve(proxyInterfaceRegistrationDefinition.Interface);
            var implementationType = TypeResolver.Resolve(proxyInterfaceRegistrationDefinition.Proxy);
            RegisterProxyComponent(
                interfaceType,
                implementationType,
                implementationType.FullName
            );
        }

        private void Register(Registration registration) {
            lock (_threadLock) {
                _registrations.Add(registration);
                _registrationsByInterfaceType.Add(registration.InterfaceType, registration);
                _registrationsByImplementationType.Add(registration.ImplementationType, registration);
                State++;
            }
        }

        #endregion

        #region Inner Classes


        public class Registration {

            public Registration(Type interfaceType, Type implementationType, string resolveKey, ActualActivationType activationType) {
                InterfaceType = interfaceType;
                ImplementationType = implementationType;
                ResolveKey = resolveKey;
                ActivationType = activationType;
            }
            public Type InterfaceType { get; set; }

            public Type ImplementationType { get; set; }

            public string ResolveKey { get; set; }

            public ActualActivationType ActivationType { get; set; }


            public static Registration From<TInterface>(TInterface instance, string name)
                where TInterface : class {
                return new Registration(typeof(TInterface), instance.GetType(), name, ActualActivationType.ExistingInstance);
            }

            public static Registration From<TInterface, TImplementation>(string name, ActivationType activationType) {
                return new Registration(typeof(TInterface), typeof(TImplementation), name, Convert(activationType));
            }

            public static Registration From(Type @interface, Type implementation, string name, ActivationType activationType) {
                return new Registration(@interface, implementation, name, Convert(activationType));
            }

            public static Registration FromProxy<TInterface, TImplementation>(string name) {
                return new Registration(typeof(TInterface), typeof(TImplementation), name, ActualActivationType.Proxy);
            }

            public static Registration FromProxy(Type @interface, Type implementation, string name) {
                return new Registration(@interface, implementation, name, ActualActivationType.Proxy);
            }

            private static ActualActivationType Convert(ActivationType activationType) {
                switch (activationType) {
                    case Application.ActivationType.Instance:
                        return ActualActivationType.NewInstance;
                    case Application.ActivationType.Singleton:
                        return ActualActivationType.Singleton;
                    case Application.ActivationType.PerRequest:
                        return ActualActivationType.PerRequest;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(activationType), activationType, null);
                }
            }
        }


        public enum ActualActivationType {
            ExistingInstance,
            NewInstance,
            Singleton,
            PerRequest,
            Proxy
        }

        #endregion

    }
}

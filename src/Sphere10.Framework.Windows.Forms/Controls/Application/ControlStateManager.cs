//-----------------------------------------------------------------------
// <copyright file="ControlStateManager.cs" company="Sphere 10 Software">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sphere10.Framework;
using Sphere10.Framework.Application;

namespace Sphere10.Framework.Windows.Forms {

    public class ControlStateManager {
        private readonly HashSet<Type> _controlStateManagers;
        private int _lastKnownComponentRegistryState;

        static ControlStateManager() {
            Instance = new ControlStateManager();
        }

        public ControlStateManager() {
            _controlStateManagers = new HashSet<Type>();
            _lastKnownComponentRegistryState = -1;
        }

        public static ControlStateManager Instance { get; }

		public IDisposable EnterUpdateScope() {
			return new ActionScope(null, Refresh);
		}

        public void Refresh() {
            _controlStateManagers.Clear();
            foreach (var registration in ComponentRegistry.Instance.Registrations.Where(r => r.InterfaceType == typeof(IControlStateChangeManager))) {
                _controlStateManagers.Add(TypeResolver.Resolve(registration.ResolveKey));
            }
            _lastKnownComponentRegistryState = ComponentRegistry.Instance.State;
        }

        public bool HasControlStateManger<TControl>() {
            if (_lastKnownComponentRegistryState != ComponentRegistry.Instance.State)
                Refresh();
            return HasControlStateManger(typeof(TControl));
        }

        public bool HasControlStateManger(Type type) {
            if (_lastKnownComponentRegistryState != ComponentRegistry.Instance.State)
                Refresh();

            return _controlStateManagers.Contains(type);
        }

        public bool TryResolveControlStateManager(Type controlType, out IControlStateChangeManager manager) {
            if (_lastKnownComponentRegistryState != ComponentRegistry.Instance.State)
                Refresh();

            if (HasControlStateManger(controlType)) {
                manager = ComponentRegistry.Instance.Resolve<IControlStateChangeManager>(controlType.FullName);
                return true;
            }
            manager = null;
            return false;
        }

    }
}

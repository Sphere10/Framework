//-----------------------------------------------------------------------
// <copyright file="ComponentRegistryForm.cs" company="Sphere 10 Software">
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
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Application;
using Sphere10.Framework.Data;
using Sphere10.Framework.Windows.Forms;

namespace Sphere10.FrameworkTester {
    public partial class ComponentRegistryForm : Form {
        private readonly ILogger _outputTextLogger;
        public ComponentRegistryForm() {
            InitializeComponent();
            _outputTextLogger = new TextBoxLogger(_outputTextBox); 
        }

        private ComponentRegistryDefinition CreateComponentRegistry2() {
            return new ComponentRegistryDefinition {
                PluginFolder = "plugins/",                
                RegistrationsDefinition = new[] {
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition {
                            Dll = "folder/mydll.dll",
                            Interface = "My.Name.Space.IInterface",
                            Implementation = "My.Name.Space.Implementation",
                            Activation = ActivationType.Instance
                        },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition {
                            Dll = "folder/mydll.dll",
                            Interface = "My.Name.Space.IInterface2",
                            Implementation = "My.Name.Space.Implementation2",
                            Activation = ActivationType.Instance
                        },
                    }
            };
        }


        private ComponentRegistryDefinition CreateComponentRegistry() {
            return new ComponentRegistryDefinition {
                RegistrationsDefinition = new ComponentRegistryDefinition.RegistrationDefinition[] {
                        new ComponentRegistryDefinition.AssemblyRegistrationDefinition() { Dll = "Sphere10.Framework.Application"},
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IBackgroundLicenseVerifier", Implementation = "StandardBackgroundLicenseVerifier"},
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ISettingsServices", Implementation = "StandardUserSettingsProvider", ResolveKey = "UserSettings", Activation = ActivationType.Singleton },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ISettingsServices", Implementation = "StandardSystemSettingsProvider", ResolveKey = "SystemSettings", Activation = ActivationType.Singleton },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IConfigurationServices", Implementation = "StandardConfigurationServices", Activation = ActivationType.Singleton },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IDuplicateProcessDetector", Implementation = "StandardDuplicateProcessDetector" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IHelpServices", Implementation = "StandardHelpServices" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ILicenseEnforcer", Implementation = "StandardLicenseEnforcer", Activation = ActivationType.Singleton},
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ILicenseKeyDecoder", Implementation = "StandardLicenseKeyDecoder" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ILicenseKeyValidator", Implementation = "StandardLicenseKeyValidatorWithVersionCheck" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ILicenseKeyEncoder", Implementation = "DefaultLicenseKeyEncoder" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ILicenseKeyServices", Implementation = "StandardLicenseKeyProvider" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "ILicenseServices", Implementation = "StandardLicenseServices", Activation = ActivationType.Singleton },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IProductInformationServices", Implementation = "StandardProductInformationServices", Activation = ActivationType.Singleton },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IProductInstancesCounter", Implementation = "StandardProductInstancesCounter" },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IProductUsageServices", Implementation = "StandardProductUsageProvider", Activation = ActivationType.Singleton },
                        new ComponentRegistryDefinition.ComponentRegistrationDefinition { Interface = "IWebsiteLauncher", Implementation = "StandardWebsiteLauncher"},

                        new ComponentRegistryDefinition.ComponentSetRegistrationDefinition {
                            Interface = "IApplicationInitializeTask",
                            RegistrationsDefinition  = new [] {
                                new ComponentRegistryDefinition.ComponentRegistrationDefinition { Implementation = "StandardProductUsageProvider.Initializer" },
                                new ComponentRegistryDefinition.ComponentRegistrationDefinition { Implementation = "IncrementUsageByOneTask" },
                                new ComponentRegistryDefinition.ComponentRegistrationDefinition { Implementation = "RegisterSettingsViaIocTask" }
                            }
                        },

                        new ComponentRegistryDefinition.ComponentSetRegistrationDefinition {
                            Interface = "IApplicationStartTask",
                            RegistrationsDefinition  = new ComponentRegistryDefinition.RegistrationDefinition[] {
                                new ComponentRegistryDefinition.ComponentRegistrationDefinition { Implementation = "SaveSettingsEndTask.Initializer" },
                            }
                        },

                        new ComponentRegistryDefinition.ComponentSetRegistrationDefinition {
                            Interface = "IApplicationEndTask",
                            RegistrationsDefinition  = new ComponentRegistryDefinition.RegistrationDefinition[] {
                                new ComponentRegistryDefinition.ComponentRegistrationDefinition { Implementation = "SaveSettingsEndTask.Initializer" },
                            }
                        }
                    }
            };            
        }

        #region Event Handlers

        private void _serializeButton_Click(object sender, EventArgs e) {
            try {
                var cr = CreateComponentRegistry();
                _outputTextBox.Text = XmlProvider.WriteToString(cr);
            } catch (Exception error) {
                _outputTextBox.Clear();
                _outputTextLogger.LogException(error);
            }
        }

        private void _deserializeButton_Click(object sender, EventArgs e) {
            try {
                var componentRegistry = CreateComponentRegistry();
                var serialized = XmlProvider.WriteToString(componentRegistry);
                var deserialized = XmlProvider.ReadFromString<ComponentRegistryDefinition>(serialized);
                _outputTextBox.Clear();
                var reserialized = XmlProvider.WriteToString(deserialized);
                if (reserialized != serialized) {
                    _outputTextBox.AppendText("Deserialization did not match - " + Environment.NewLine);
                    _outputTextBox.AppendText(reserialized);
                } else {
                    _outputTextBox.AppendText("Passed");
                }
            } catch (Exception error) {
                _outputTextBox.Clear();
                _outputTextLogger.LogException(error);
            }
        }

        private void _registerButton_Click(object sender, EventArgs e) {
            try {

            } catch (Exception error) {
                _outputTextBox.Clear();
                _outputTextLogger.LogException(error);
            }
        }
        

        private void _loadConfigButton_Click(object sender, EventArgs e) {
            try {
                ApplicationLifecycle.Instance.RegisterAppConfig();
                _outputTextBox.AppendText("Passed");
            } catch (Exception error) {
                _outputTextBox.Clear();
                _outputTextLogger.LogException(error);
            }
        }

        #endregion
    }
}

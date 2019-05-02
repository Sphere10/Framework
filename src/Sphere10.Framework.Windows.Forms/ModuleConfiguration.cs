//-----------------------------------------------------------------------
// <copyright file="ModuleConfiguration.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Application;
using Sphere10.Framework.Windows;

namespace Sphere10.Framework.Windows.Forms {
    public class ModuleConfiguration : ModuleConfigurationBase {
        public override void RegisterComponents(ComponentRegistry registry) {

            if (!registry.HasImplementationFor<IAboutBox>())
                registry.RegisterComponent<IAboutBox, StandardAboutBox>();

            if (!registry.HasImplementationFor<INagDialog>())
                registry.RegisterComponent<INagDialog, StandardNagDialog>();

            if (!registry.HasImplementationFor<IReportBugDialog>())
                registry.RegisterComponent<IReportBugDialog, StandardReportBugDialog>();

            if (!registry.HasImplementationFor<IRequestFeatureDialog>())
                registry.RegisterComponent<IRequestFeatureDialog, StandardRequestFeatureDialog>();

            if (!registry.HasImplementationFor<ISendCommentsDialog>())
                registry.RegisterComponent<ISendCommentsDialog, StandardSendCommentsDialog>();

            if (!registry.HasImplementationFor<IActiveApplicationMonitor>())
                registry.RegisterComponent<IActiveApplicationMonitor, PollDrivenActiveApplicationMonitor>();

            if (!registry.HasImplementationFor<IMouseHook>())
                registry.RegisterComponent<IMouseHook, WindowsMouseHook>(ActivationType.Singleton);

            if (!registry.HasImplementationFor<IKeyboardHook>())
                registry.RegisterComponent<IKeyboardHook, WindowsKeyboardHook>(ActivationType.Singleton);

			//if (registry.Registrations.Count(r =>
			//	r.InterfaceType == typeof(IBackgroundLicenseVerifier) &&
			//		r.ImplementationType == typeof(NoOpBackgroundLicenseVerifier)) == 1) {
			//	registry.RegisterComponent<IBackgroundLicenseVerifier, BITSBackgroundLicenseVerifier>();
			//} else {
   //             throw new SoftwareException("Illegal tampering with IBackgroundLicenseVerifier");
   //         }

            if (!registry.HasImplementationFor<IAutoRunServices>())
                registry.RegisterComponent<IAutoRunServices, StartupFolderAutoRunServicesProvider>();

			// This is the primary form of the application, so register it as a provider of the below services
			var hasAppControl = ControlStateManager.Instance.HasControlStateManger<ApplicationControl>();
	        var hasNumUpDown = ControlStateManager.Instance.HasControlStateManger<NumericUpDown>();
	        var hasTextBox = ControlStateManager.Instance.HasControlStateManger<TextBox>();
	        var hasComboBox = ControlStateManager.Instance.HasControlStateManger<ComboBox>();
	        var hasRadioButton = ControlStateManager.Instance.HasControlStateManger<RadioButton>();
	        var hasCheckBox = ControlStateManager.Instance.HasControlStateManger<CheckBox>();
	        var hasCheckedListBox = ControlStateManager.Instance.HasControlStateManger<CheckedListBox>();
	        var hasDateTimePicker = ControlStateManager.Instance.HasControlStateManger<DateTimePicker>();
			if (!hasAppControl)
				registry.RegisterControlStateManager<ApplicationControl, DefaultControlStateChangeManager>();

			if (!hasNumUpDown)
				registry.RegisterControlStateManager<NumericUpDown, DefaultControlStateChangeManager>();

			if (!hasTextBox)
				registry.RegisterControlStateManager<TextBox, DefaultControlStateChangeManager>();

			if (!hasComboBox)
				registry.RegisterControlStateManager<ComboBox, DefaultControlStateChangeManager>();

			if (!hasRadioButton)
				registry.RegisterControlStateManager<RadioButton, DefaultControlStateChangeManager>();

			if (!hasCheckBox)
				registry.RegisterControlStateManager<CheckBox, DefaultControlStateChangeManager>();

			if (!hasCheckedListBox)
				registry.RegisterControlStateManager<CheckedListBox, DefaultControlStateChangeManager>();

			if (!hasDateTimePicker)
				registry.RegisterControlStateManager<DateTimePicker, DefaultControlStateChangeManager>();
			

            // Initialize Tasks
            if (!registry.HasImplementation<SessionEndingHandlerTask>())
                registry.RegisterInitializationTask<SessionEndingHandlerTask>();

            // Start Tasks

            // End Tasks

        }

    }
}

// To override use below as guide:
//<ComponentRegistry>
//    <!-- Assemblies to pre-load -->
//    <Assembly dll = "Sphere10.Framework.Application.dll" />
//    < Assembly dll="Sphere10.Framework.Windows.Forms.dll" />
//    <Assembly dll = "Sphere10.Framework.Windows.Forms.dll" />
//    < Assembly dll="Sphere10.Framework.Windows.Forms.Sqlite.dll" />
//    <Assembly dll = "BlockchainSQL.DataAccess.NHibernate" />

//    <!--Sphere 10 Framework Components -->
//    <Component interface="IBackgroundLicenseVerifier"   implementation="StandardBackgroundLicenseVerifier" />
//    <Component interface="ISettingsServices"            implementation="StandardUserSettingsProvider"     activation="Singleton" resolveKey="UserSettings" />
//    <Component interface="ISettingsServices"            implementation="StandardSystemSettingsProvider"   activation="Singleton" resolveKey="SystemSettings" />
//    <Component interface="IConfigurationServices"       implementation="StandardConfigurationServices"    activation="Singleton" />
//    <Component interface="IDuplicateProcessDetector"    implementation="StandardDuplicateProcessDetector" />
//    <Component interface="IHelpServices"                implementation="StandardHelpServices" />
//    <Component interface="ILicenseEnforcer"             implementation="StandardLicenseEnforcer"          activation="Singleton" />
//    <Component interface="ILicenseKeyDecoder"           implementation="StandardLicenseKeyDecoder" />
//    <Component interface="ILicenseKeyValidator"         implementation="StandardLicenseKeyValidatorWithVersionCheck" />
//    <Component interface="ILicenseKeyEncoder"           implementation="StandardLicenseKeyEncoder" />
//    <Component interface="ILicenseKeyServices"          implementation="StandardLicenseKeyProvider" />
//    <Component interface="ILicenseServices"             implementation="StandardLicenseServices"          activation="Singleton" />
//    <Component interface="IProductInformationServices"  implementation="StandardProductInformationServices" activation="Singleton" />
//    <Component interface="IProductInstancesCounter"     implementation="StandardProductInstancesCounter" />
//    <Component interface="IProductUsageServices"        implementation="StandardProductUsageProvider"     activation="Singleton" />
//    <Component interface="IWebsiteLauncher"             implementation="StandardWebsiteLauncher" />

//    <!-- Sphere 10 WinForms Framework Components -->
//    <Component interface="IAboutBox"                    implementation="StandardAboutBox" />
//    <Component interface="INagDialog"                   implementation="StandardNagDialog" />
//    <Component interface="IReportBugDialog"             implementation="StandardReportBugDialog" />
//    <Component interface="IRequestFeatureDialog"        implementation="StandardRequestFeatureDialog" />
//    <Component interface="ISendCommentsDialog"          implementation="StandardSendCommentsDialog" />
//    <Component interface="IActiveApplicationMonitor"    implementation="PollDrivenActiveApplicationMonitor" />
//    <Component interface="IMouseHook"                   implementation="WindowsMouseHook" Activation="Singleton" />
//    <Component interface="IKeyboardHook"                implementation="WindowsKeyboardHook" Activation="Singleton" />
//    <Component interface="IBackgroundLicenseVerifier"   implementation="BITSBackgroundLicenseVerifier" />
//    <Component interface="IAutoRunServices"             implementation="StartupFolderAutoRunServicesProvider" />
//    <ComponentSet interface="IControlStateChangeManager">
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="Sphere10.Framework.Windows.Forms.ApplicationControl" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.NumericUpDown" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.TextBox" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.ComboBox" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.RadioButton" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.CheckBox" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.CheckedListBox" />
//      <Component implementation = "CommonControlsControlStateChangeManager"   resolveKey="System.Windows.Forms.DateTimePicker" />
//    </ComponentSet>
    
//    <!-- Application Initialize Tasks -->
//    <ComponentSet interface="IApplicationInitializeTask">
//      <Component implementation = "StandardProductUsageProvider+Initializer" />
//      < Component implementation="IncrementUsageByOneTask" />
//      <Component implementation = "RegisterSettingsViaIocTask" />
//      < Component implementation="SessionEndingHandlerTask" />
//    </ComponentSet>

//    <!-- Application Start Tasks -->
//    <ComponentSet interface="IApplicationStartTask">
//    </ComponentSet>

//    <!-- Application End Tasks-->
//    <ComponentSet interface="IApplicationEndTask">
//      <Component implementation = "SaveSettingsEndTask" />
//    </ComponentSet >


//    <!--Application Main Form -->
//    <Component interface="IMainForm"             implementation="BlockchainSQL.Server.MainForm"  Activation="Singleton" />
//    <Proxy interface="IApplicationIconProvider"  proxy="IMainForm" />
//    <Proxy interface="IUserInterfaceServices"    proxy="IMainForm" />
//    <Proxy interface="IUserInterfaceServices"    proxy="IMainForm" />
//    <Proxy interface="IUserNotificationServices" proxy="IMainForm" />
//    <Proxy interface="IApplicationIconProvider"  proxy="IMainForm" />
//    <!--Proxy interface="IBlockManager"  proxy="IMainForm" /-->
    
    
//  </ComponentRegistry>

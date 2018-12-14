//-----------------------------------------------------------------------
// <copyright file="ApplicationControl.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

using System.Windows.Forms;
using System.Globalization;
using Sphere10.Application.WinForms.Controls;
using Sphere10.Framework;

namespace Sphere10.Application.WinForms {

	/// <summary>
	/// A base class for all controls in the application. Provides access to application services and 
	/// features like automatically detect child control state changes. Draws theme-aware borders.
	/// </summary>
	public partial class ApplicationControl : UserControl {
		public event EventHandler StateChanged;

		public ApplicationControl() {
			ApplicationServices = new WinFormsApplicationServices();
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			if (!Tools.Runtime.IsDesignMode) {
				try {
					BeginUpdate();
					RegisterStateChangeListeners();
					if (AutoLocateSettings) {
						var useSettingsAttribute = this.GetType().GetCustomAttributesOfType<UseSettingsAttribute>().SingleOrDefault();
						if (useSettingsAttribute == null) {
							throw new SoftwareException(
								"Unable to locate settings for component {0}. Disable the 'AutoLocateSettings' setting on the component or add a 'UseSettingsAttribute' attribute to class declaration.");
						}
						LocatedSettings = ApplicationServices.GetComponentSettings(useSettingsAttribute.SettingsTypeToUse);
					}
					PopulatePrimingData();
					RefreshUserInterfaceWithDataSource();
				} finally {
					EndUpdate();
				}
				Loaded = true;
			}
			
		}

		public virtual void SetLocalizedText(CultureInfo culture = null) {
		}

		protected virtual void PopulatePrimingData() {
		}

		protected virtual void OnStateChanged() {
		}

		protected virtual void SaveUserInputToDataSource() {
		}

		protected virtual void RefreshUserInterfaceWithDataSource() {
		}

		protected virtual void BeginUpdate() {
			Updating = true;
		}

		protected virtual void EndUpdate(bool saveToDataSource = false) {
			Updating = false;
			FireStateChangedEvent(saveToDataSource);
		}

		public void Refresh() {
			try {
				BeginUpdate();
				RefreshUserInterfaceWithDataSource();
			} finally {
				EndUpdate();
			}
		}

		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(true)]
		public virtual bool AutoDetectChildStateChanges { get; set; }

		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(false)]
		public virtual bool AutoSaveSettingsOnStateChanged { get; set; }

		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(false)]
		public virtual bool AutoLocateSettings { get; set; }

    	[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected IWinFormsApplicationServices ApplicationServices { get; private set; }

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Updating { get; private set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual ComponentSettings LocatedSettings { get; private set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected bool Loaded { get; private set; }
      
		protected void FireStateChangedEvent(bool saveToDataSource = false) {
			if (!Updating) {
				if (AutoDetectChildStateChanges && Loaded) {
					OnStateChanged();
					if (StateChanged != null) {
						StateChanged(this, new EventArgs());
					}
				}
				if (saveToDataSource) {
					SaveUserInputToDataSource();
					if (AutoSaveSettingsOnStateChanged) {
						if (LocatedSettings != null) {
							LocatedSettings.Save();
						}
					}
				}
			}
		}


		/// <summary>
        /// This control monitors all the state change events of its child controls
        /// </summary>
        /// <param name="e"></param>
		protected override void OnControlAdded(ControlEventArgs e) {
			if (!Tools.Runtime.IsDesignMode) {
				AddStateChangedHandler(e.Control);
				base.OnControlAdded(e);
			}
		}

		private void RegisterStateChangeListeners() {
			Controls.Cast<Control>().ForEach(AddStateChangedHandler);
		}


		private void ChildControl_StateChangedHandler(object sender, EventArgs e) {
			if (!Updating) {
				FireStateChangedEvent(AutoSaveSettingsOnStateChanged);
			}
		}


		#region Auxillary Methods

		private void AddStateChangedHandler(Control control) {
            IControlStateChangeManager stateChangeManager;
            if (ControlStateManager.Instance.TryResolveControlStateManager(control.GetType(), out stateChangeManager)) {
                stateChangeManager.AddStateChangedHandler(control, new EventHandler(ChildControl_StateChangedHandler));
            } else {
                // its probably a container control, so process its child controls
                foreach (Control child in control.Controls) {
                    AddStateChangedHandler(child);
                }
            }
		}

		#endregion



	}

}

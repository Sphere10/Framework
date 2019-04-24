//-----------------------------------------------------------------------
// <copyright file="ApplicationScreen.cs" company="Sphere 10 Software">
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
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using Sphere10.Framework.Application;
using Sphere10.Framework.Windows.Forms;


namespace Sphere10.Framework.Windows.Forms {

    /// <summary>
    /// Application UIs can be presented as a screen. It is a proxy ApplicationServiceProvider
    /// routing to parent provider. 
    /// 
    /// NOTE: The ApplicationServiceProvider property, which defines the underlying provider all
    /// such  calls are routed to, is guaranteed to be set post-construction.
    /// </summary>
    public class ApplicationScreen : ApplicationControl, IUpdatable, IHelpableObject {
	    public event EventHandler StateChanged;
		private List<ToolStripItem> _menuStripItems;

        public ApplicationScreen()
            : this(null) {
        }

        public ApplicationScreen(IApplicationBlock applicationBlock) {
            ApplicationBlock = applicationBlock;
			Url = FileName = null;
			Type = HelpType.None;
            _menuStripItems = new List<ToolStripItem>();
        }

		[Browsable(true), Category("Appearance")]
		public string ApplicationMenuStripText { get; set; }

		[Browsable(true), Category("Appearance")]
		public bool ShowInApplicationMenuStrip { get; set; }

        [Browsable(true), Category("Layout"), Description("How this screen will be displayed to the user")]
        public ScreenDisplayMode DisplayMode { get; set; }

        [Browsable(true), Category("Behavior"), Description("How this screen will be displayed to the user")]
        public ScreenActivationMode ActivationMode { get; set; }

        [Browsable(false)]
        public IApplicationBlock ApplicationBlock { get; set; }

	    [Category("Behavior")]
	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	    [DefaultValue(true)]
	    public bool AutoDetectChildStateChanges { get; set; }

	    [Category("Behavior")]
	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	    [DefaultValue(true)]
	    public bool AutoSave { get; set; }


	    [Browsable(false)]
	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	    public bool Updating { get; set; }

	    [Browsable(false)]
	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	    public bool Loaded { get; internal set; }


		//[Browsable(false)]
  //      public bool Dirty { get; set; }
        
        /// <summary>
        /// The menu items associated with this screen.
        /// </summary>
        [Browsable(false)]
        public ToolStripItem[] MenuItems {
            get {
                return _menuStripItems.ToArray();
            }
        }

        /// <summary>
        /// The toolbar associated with this screen.
        /// </summary>
        [Browsable(true), Category("Behavior"), Description("The toolbar associated with this screen.")]
        public ToolStrip ToolBar { get; set; }


	    public HelpType Type {
		    get;
		    private set;
	    }

	    public string FileName {
		    get;
		    private set;
	    }

	    public string Url {
		    get;
		    private set;
	    }

	    public int? PageNumber {
		    get;
		    private set;
	    }

	    public int? HelpTopicID {
		    get;
		    private set;
	    }

	    public int? HelpTopicAlias {
		    get;
		    private set;
	    }

		public virtual void SetLocalizedText() {
            SetLocalizedTextInApplicationControls(this.Controls);
        }

	    internal virtual void OnCreateScreen() {
        }

	    internal virtual void OnShowScreen() {
        }

	    internal virtual void OnHideScreen(ref bool cancelHide) {
        }

	    internal virtual void OnDestroyScreen() {
	    }

	    internal virtual void OnStateChanged() {
	    }

		internal virtual void PopulatePrimingData() {
	    }

		internal virtual void SaveState() {
	    }

	    internal virtual void SaveUserInputToDataSource() {
	    }

	    internal virtual void RefreshUserInterfaceWithDataSource() {
	    }


        protected void RegisterMenuItem(ToolStripItem item) {
            _menuStripItems.Add(item);
        }

        private void SetLocalizedTextInApplicationControls(ControlCollection controls) {
            if (controls != null) {
                foreach (Control control in controls) {
                    if (control is ApplicationControl) {
                        ((ApplicationControl)control).SetLocalizedText();
                    }
                    SetLocalizedTextInApplicationControls(
                        control.Controls
                    );
                }
            }
        }

	    public void NotifyStateChangedEvent(bool saveToDataSource = false) {
			if (!Updating) {
				if (AutoDetectChildStateChanges) {
					OnStateChanged();
					StateChanged?.Invoke(this, new EventArgs());
				}
				if (saveToDataSource) {
					SaveUserInputToDataSource();
					if (AutoSave) {
						SaveState();
					}
				}
			}
		}
	
	}
}


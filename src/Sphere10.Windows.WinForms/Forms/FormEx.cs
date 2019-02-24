//-----------------------------------------------------------------------
// <copyright file="FormEx.cs" company="Sphere 10 Software">
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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Sphere10.Framework;

namespace Sphere10.Windows.WinForms {

	public class FormEx : Form, IStateChangeObserver, IUpdatable {
		public event EventHandler StateChanged;

		public FormEx() {
			CloseAction = FormCloseAction.Close;
			Updating = false;
			Loaded = false;
			OpacityBeforeHide = 1;
		}

		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(FormCloseAction.Close)]
		public FormCloseAction CloseAction { get; set; }

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
		public bool Loaded { get; private set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private double OpacityBeforeHide { get; set; }

		protected override void OnLoad(EventArgs e) {
			try {
				base.OnLoad(e);
				if (DesignMode || Loaded)
					return;

				using (this.EnterUpdateScope()) {
					RegisterStateChangeListeners();
					PopulatePrimingData();
					RefreshUserInterfaceWithDataSource();
				}
				Loaded = true;
			} catch(Exception error) {
				MessageBox.Show(this, error.ToDiagnosticString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public virtual void SetLocalizedText(CultureInfo culture = null) {
		}

		public new virtual void Refresh() {
			using(this.EnterUpdateScope()) {
				RefreshUserInterfaceWithDataSource();
			}
			base.Refresh();
		}

		public virtual new void Show() {
			if (WindowState == FormWindowState.Minimized) {
				TopMost = true;
				TopMost = false;
				Visible = true;
				WindowState = FormWindowState.Normal;
			} else {
				TopMost = true;
				TopMost = false;
				Visible = true;
			}
			if (Math.Abs(Opacity) < 0.00001D)  // if Opacity == 0
				Opacity = OpacityBeforeHide;
		}

		public virtual new void Hide() {
			OpacityBeforeHide = Opacity;
			Opacity = 0;
			Visible = false;
		}

		protected virtual void PopulatePrimingData() {
		}

		protected virtual void OnStateChanged() {
		}

		protected virtual void SaveState() {
		}

		protected virtual void SaveUserInputToDataSource() {
		}

		protected virtual void RefreshUserInterfaceWithDataSource() {
		}

		public void NotifyStateChangedEvent(bool saveToDataSource = false) {
			if (!Updating) {
				if (AutoDetectChildStateChanges) {
					OnStateChanged();
					if (StateChanged != null) {
						StateChanged(this, new EventArgs());
					}
				}
				if (saveToDataSource) {
					SaveUserInputToDataSource();
					if (AutoSave) {
						SaveState();
					}
				}
			}
		}

		#region Auxillary Methods

		/// <summary>
		/// This control monitors all the state change events of its child controls
		/// </summary>
		/// <param name="e"></param>
		protected override void OnControlAdded(ControlEventArgs e) {
			if (!DesignMode) {
				AddStateChangedHandler(e.Control);
				base.OnControlAdded(e);
			}
		}

		protected override void OnClosing(CancelEventArgs e) {
			if (!CloseAction.HasFlag(FormCloseAction.Close)) {
				e.Cancel = true;
				if (CloseAction.HasFlag(FormCloseAction.Hide)) {
					Hide();
				}
				if (CloseAction.HasFlag(FormCloseAction.Minimize)) {
					WindowState = FormWindowState.Minimized;
				}
			}
		}

		private void RegisterStateChangeListeners() {
			Controls.Cast<Control>().ForEach(AddStateChangedHandler);
		}

		private void ChildControl_StateChangedHandler(object sender, EventArgs e) {
			if (!Updating) {
				NotifyStateChangedEvent(AutoSave);
			}
		}

		private void AddStateChangedHandler(Control control) {
			if (!IsBeingObserved(control)) {
				if (CanObserve(control)) {
					Observe(control, ChildControl_StateChangedHandler);
				} else {
					// its probably a container control, so process its child controls
					foreach (Control child in control.Controls) {
						AddStateChangedHandler(child);
					}
				}
			}
		}

		#endregion

		#region State Change Observer

		readonly ISet<object> _observedObjects = new HashSet<object>();  

		public bool IsBeingObserved(object source) {
			return _observedObjects.Contains(source);
		}

		public bool CanObserve(object source) {
			bool canObserve = false;
			TypeSwitch.Do(source,
				TypeSwitch.Case<IStateChangeEventSource>( () => canObserve = true),
				TypeSwitch.Case<NumericUpDown>( () => canObserve = true),
				TypeSwitch.Case<IStateChangeEventSource>( () => canObserve = true),
				TypeSwitch.Case<TextBox>( () => canObserve = true),
				TypeSwitch.Case<ComboBox>( () => canObserve = true),
				TypeSwitch.Case<RadioButton>( () => canObserve = true),
				TypeSwitch.Case<CheckBox>( () => canObserve = true),
				TypeSwitch.Case<CheckedListBox>( () => canObserve = true),
				TypeSwitch.Case<DateTimePicker>( () => canObserve = true),
				TypeSwitch.Case<ListBox>( () => canObserve = true),
				TypeSwitch.Default(() => canObserve = false)
			);
			return canObserve;
		}


		public void Observe(object source, EventHandler eventHandler) {
			if (source is IStateChangeEventSource) {
				((IStateChangeEventSource)source).AddObserver(this, eventHandler);
			} else if (source is NumericUpDown) {
				((NumericUpDown)source).ValueChanged += eventHandler;
			} else if (source is TextBox) {
				((TextBox)source).TextChanged += eventHandler;
			} else if (source is ComboBox) {
				((ComboBox)source).SelectedIndexChanged += eventHandler;
			} else if (source is RadioButton) {
				((RadioButton)source).CheckedChanged += eventHandler;
			} else if (source is CheckBox) {
				((CheckBox)source).CheckedChanged += eventHandler;
			} else if (source is CheckedListBox) {
				((CheckedListBox)source).SelectedIndexChanged += eventHandler;
			} else if (source is DateTimePicker) {
				((DateTimePicker)source).TextChanged += eventHandler;
			} else if (source is ListBox) {
				((ListBox)source).SelectedIndexChanged += eventHandler;
			} else {
				throw new SoftwareException("CommonControlsControlStateChangeManager does not process types", source.GetType().Name);
			}
			_observedObjects.Add(source);
		}

		public void Unobserve(object source, EventHandler eventHandler) {
			if (source is IStateChangeEventSource) {
				((IStateChangeEventSource)source).RemoveObserver(this, eventHandler);
			} else if (source is NumericUpDown) {
				((NumericUpDown)source).ValueChanged -= eventHandler;
			} else if (source is TextBox) {
				((TextBox)source).TextChanged -= eventHandler;
			} else if (source is ComboBox) {
				((ComboBox)source).SelectedIndexChanged -= eventHandler;
			} else if (source is RadioButton) {
				((RadioButton)source).CheckedChanged -= eventHandler;
			} else if (source is CheckBox) {
				((CheckBox)source).CheckedChanged -= eventHandler;
			} else if (source is CheckedListBox) {
				((CheckedListBox)source).SelectedIndexChanged -= eventHandler;
			} else if (source is DateTimePicker) {
				((DateTimePicker)source).TextChanged -= eventHandler;
			} else if (source is ListBox) {
				((ListBox)source).SelectedIndexChanged -= eventHandler;
			} else {
				throw new SoftwareException("CommonControlsControlStateChangeManager does not process types", source.GetType().Name);
			}
			_observedObjects.Remove(source);
		}


		#endregion

	}


}

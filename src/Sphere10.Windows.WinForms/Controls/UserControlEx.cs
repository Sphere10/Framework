//-----------------------------------------------------------------------
// <copyright file="UserControlEx.cs" company="Sphere 10 Software">
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

	public class UserControlEx : UserControl, IStateChangeObserver, IUpdatable {
		public event EventHandler StateChanged;
		public UserControlEx() {
			Updating = false;
		}

		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(true)]
		public bool AutoDetectChildStateChanges { get; set; }

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Updating { get;  set; }

		protected virtual void OnStateChanged() {
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


		private void RegisterStateChangeListeners() {
			Controls.Cast<Control>().ForEach(AddStateChangedHandler);
		}

		private void ChildControl_StateChangedHandler(object sender, EventArgs e) {
			if (!Updating) {
				NotifyStateChangedEvent(false);
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

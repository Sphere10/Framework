//-----------------------------------------------------------------------
// <copyright file="CommonControlStateChangeManager.cs" company="Sphere 10 Software">
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
using System.Drawing;
using System.Windows.Forms;
using Sphere10.Framework.Application;
using Sphere10.Framework;

namespace Sphere10.Framework.WinForms {

	public class CommonControlsControlStateChangeManager : IControlStateChangeManager {

	    public void AddStateChangedHandler(Control control, EventHandler eventHandler) {
			if (control is ApplicationControl) {
				((ApplicationControl)control).StateChanged += eventHandler;
			} else if (control is NumericUpDown) {
				((NumericUpDown)control).ValueChanged += eventHandler;
			} else if (control is TextBox) {
				((TextBox)control).TextChanged += eventHandler;
			} else if (control is ComboBox) {
				((ComboBox)control).SelectedIndexChanged += eventHandler;
			} else if (control is RadioButton) {
				((RadioButton)control).CheckedChanged += eventHandler;
			} else if (control is CheckBox) {
				((CheckBox)control).CheckedChanged += eventHandler;
			} else if (control is CheckedListBox) {
				((CheckedListBox)control).SelectedIndexChanged += eventHandler;
			} else if (control is DateTimePicker) {
				((DateTimePicker)control).TextChanged += eventHandler;
			} else if (control is ListBox) {
				((ListBox)control).SelectedIndexChanged += eventHandler;
			} else {
				throw new SoftwareException("CommonControlsControlStateChangeManager does not process types", control.GetType().Name);
			}
		}

		public void RemoveStateChangedHandler(Control control, EventHandler eventHandler) {
			if (control is ApplicationControl) {
				((ApplicationControl)control).StateChanged -= eventHandler;
			} else if (control is NumericUpDown) {
				((NumericUpDown)control).ValueChanged -= eventHandler;
			} else if (control is TextBox) {
				((TextBox)control).TextChanged -= eventHandler;
			} else if (control is ComboBox) {
				((ComboBox)control).SelectedIndexChanged -= eventHandler;
			} else if (control is RadioButton) {
				((RadioButton)control).CheckedChanged -= eventHandler;
			} else if (control is CheckBox) {
				((CheckBox)control).CheckedChanged -= eventHandler;
			} else if (control is CheckedListBox) {
				((CheckedListBox)control).SelectedIndexChanged -= eventHandler;
			} else if (control is DateTimePicker) {
				((DateTimePicker)control).TextChanged -= eventHandler;
			} else if (control is ListBox) {
				((ListBox)control).SelectedIndexChanged -= eventHandler;
			} else {
				throw new SoftwareException("CommonControlsControlStateChangeManager does not process types", control.GetType().Name);
			}
			
		}




	}
}

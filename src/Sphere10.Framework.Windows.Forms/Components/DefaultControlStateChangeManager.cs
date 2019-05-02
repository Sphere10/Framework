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
using System.Linq;
using System.Windows.Forms;
using Sphere10.Framework.Application;
using Sphere10.Framework;

namespace Sphere10.Framework.Windows.Forms {

	public class DefaultControlStateChangeManager : IControlStateChangeManager {

		public void AddStateChangedHandler(Control control, EventHandler eventHandler) {
			TypeSwitch.Do(
				control,
				TypeSwitch.Case<ApplicationControl>(c => c.StateChanged += eventHandler),
				TypeSwitch.Case<NumericUpDown>(c => c.ValueChanged += eventHandler),
				TypeSwitch.Case<TextBox>(c => c.TextChanged += eventHandler),
				TypeSwitch.Case<ComboBox>(c => c.SelectedIndexChanged += eventHandler),
				TypeSwitch.Case<RadioButton>(c => c.CheckedChanged += eventHandler),
				TypeSwitch.Case<CheckBox>(c => c.CheckedChanged += eventHandler),
				TypeSwitch.Case<CheckedListBox>(c => c.SelectedIndexChanged += eventHandler),
				TypeSwitch.Case<DateTimePicker>(c => c.TextChanged += eventHandler),
				TypeSwitch.Case<ListBox>(c => c.SelectedIndexChanged += eventHandler),
				TypeSwitch.Default( () => {
					  throw new SoftwareException($"Control '{control.GetType().Name}' is not supported");
				})
			);
		}

		public void RemoveStateChangedHandler(Control control, EventHandler eventHandler) {
			TypeSwitch.Do(
				control,
				TypeSwitch.Case<ApplicationControl>(c => c.StateChanged -= eventHandler),
				TypeSwitch.Case<NumericUpDown>(c => c.ValueChanged -= eventHandler),
				TypeSwitch.Case<TextBox>(c => c.TextChanged -= eventHandler),
				TypeSwitch.Case<ComboBox>(c => c.SelectedIndexChanged -= eventHandler),
				TypeSwitch.Case<RadioButton>(c => c.CheckedChanged -= eventHandler),
				TypeSwitch.Case<CheckBox>(c => c.CheckedChanged -= eventHandler),
				TypeSwitch.Case<CheckedListBox>(c => c.SelectedIndexChanged -= eventHandler),
				TypeSwitch.Case<DateTimePicker>(c => c.TextChanged -= eventHandler),
				TypeSwitch.Case<ListBox>(c => c.SelectedIndexChanged -= eventHandler),
				TypeSwitch.Default(() => {
					throw new SoftwareException($"Control '{control.GetType().Name}' is not supported");
				})
			);
		}
	}
}

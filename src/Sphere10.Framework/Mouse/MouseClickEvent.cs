//-----------------------------------------------------------------------
// <copyright file="MouseClickEvent.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework
{

    public class MouseClickEvent : MouseEvent
    {

		public MouseClickEvent(string processName, int x, int y, MouseButton clickedButtons, MouseButtonState buttonState, MouseClickType clickType, DateTime time)
			: base(processName, x, y, time) {
				Buttons = clickedButtons;
				ButtonState = buttonState;
				ClickType = clickType;
		}
 

		public MouseButton Buttons { get; private set; }
        public MouseButtonState ButtonState { get; private set; }
		public MouseClickType ClickType { get; private set; }


    }
}



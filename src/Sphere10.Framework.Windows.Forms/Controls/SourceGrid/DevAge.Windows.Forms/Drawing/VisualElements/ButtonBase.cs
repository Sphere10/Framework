//-----------------------------------------------------------------------
// <copyright file="ButtonBase.cs" company="Sphere 10 Software">
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

namespace DevAge.Drawing.VisualElements
{
    public interface IButton : IBackground
    {
        ButtonStyle Style
        {
            get;
            set;
        }
    }

    [Serializable]
    public abstract class ButtonBase : BackgroundBase, IButton
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ButtonBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public ButtonBase(ButtonBase other)
            : base(other)
        {
            Style = other.Style;
        }
        #endregion

        #region Properties
        private ButtonStyle mControlDrawStyle = ButtonStyle.Normal;
        public virtual ButtonStyle Style
        {
            get { return mControlDrawStyle; }
            set { mControlDrawStyle = value; }
        }
        protected virtual bool ShouldSerializeStyle()
        {
            return Style != ButtonStyle.Normal;
        }
        #endregion

    }
}

//-----------------------------------------------------------------------
// <copyright file="ListMerger.cs" company="Sphere 10 Software">
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Sphere10.Common {


    public enum ListMergerSide {
        Left,
        Right
    }

    public delegate void ItemSelectedHandler(ListMerger source, ListMergerSide side, object selectedItem);

    public delegate void ItemsMovedHandler(ListMerger source, ListMergerSide from, ListMergerSide to, object[] item);

    public partial class ListMerger : UserControl {

        public ListMerger() {
            InitializeComponent();
        }

        #region Events

        public event ItemSelectedHandler ItemSelected;
        public event ItemsMovedHandler ItemsMoved;

        #endregion

        #region Properties

        public string LeftHeader {
            get {
                return _leftHeaderLabel.Text;
            }
            set {
                _leftHeaderLabel.Text = value;
            }
        }

        public string RightHeader {
            get {
                return _rightHeaderLabel.Text;
            }
            set {
                _rightHeaderLabel.Text = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] LeftList {
            get {
				return _leftListBox.Items.Cast<object>().ToArray(); 
            }
            set {
                _leftListBox.Items.Clear();
                _leftListBox.Items.AddRange(value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] RightList {
            get {
                return _rightListBox.Items.Cast<object>().ToArray();
            }
            set {
                _rightListBox.Items.Clear();
                _rightListBox.Items.AddRange(value);
            }
        }

        #endregion


        #region Methods

        public void ClearLists() {
            LeftList =
                RightList =
                    new object[0];
        }

        protected virtual void OnItemMoved(ListMergerSide from, ListMergerSide to, object item) {
        }

        protected virtual void OnItemSelected(ListMergerSide side, object selectedItem) {
        }

        private void MoveToLeft() {
			object[] selectedItems = _rightListBox.SelectedItems.Cast<object>().ToArray();
            _leftListBox.Items.AddRange(selectedItems);
			_rightListBox.RemoveSelectedItems();
            FireItemsMovedEvent(ListMergerSide.Right, ListMergerSide.Left, selectedItems);
        }
        private void MoveToRight() {
			object[] selectedItems = _leftListBox.SelectedItems.Cast<object>().ToArray();
            _rightListBox.Items.AddRange(selectedItems);
			_leftListBox.RemoveSelectedItems();
            FireItemsMovedEvent(ListMergerSide.Left, ListMergerSide.Right, selectedItems);
        }

        private void FireItemsMovedEvent(ListMergerSide from, ListMergerSide to, object[] items) {
            OnItemMoved(from, to, items);
            if (ItemsMoved != null) {
                ItemsMoved(this, from, to, items);
            }

        }

        private void FireItemSelectedEvent(ListMergerSide side, object item) {
            OnItemSelected(side, item);
            if (ItemSelected != null) {
                ItemSelected(this, side, item);
            }
        }

        #endregion


        #region Handlers

        private void _moveLeftButton_Click(object sender, EventArgs e) {
            MoveToLeft();
        }

        private void _moveRightButton_Click(object sender, EventArgs e) {
            MoveToRight();
        }

        private void ListMerger_Resize(object sender, EventArgs e) {
            _leftListBox.Width = Width / 2 - 34 / 2;
            _leftListBox.Height = Height - 15;
            _leftHeaderLabel.Width = _leftListBox.Width;

            _rightListBox.Width = _leftListBox.Width;
            _rightListBox.Location = new Point(Width - _rightListBox.Width, _rightListBox.Location.Y);
            _rightListBox.Height = Height - 15;
            _rightHeaderLabel.Width = _rightListBox.Width;
            _rightHeaderLabel.Location = new Point(_rightListBox.Location.X, _rightHeaderLabel.Location.Y);


            _moveLeftButton.Location
                = new Point(
                    Width / 2 - _moveLeftButton.Width / 2,
                    Height / 2 - 6 / 2 - _moveLeftButton.Height
                  );

            _moveRightButton.Location
                = new Point(
                    Width / 2 - _moveRightButton.Width / 2,
                    Height / 2 + 6 / 2
                  );
        }

        private void _rightListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (_rightListBox.SelectedIndex != -1) {
                FireItemSelectedEvent(
                    ListMergerSide.Right,
                    _rightListBox.Items[_rightListBox.SelectedIndex]
                );
            }
        }

        private void _leftListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (_leftListBox.SelectedIndex != -1) {
                FireItemSelectedEvent(
                    ListMergerSide.Left,
                    _leftListBox.Items[_leftListBox.SelectedIndex]
                );
            }
        }

        #endregion


    }

}

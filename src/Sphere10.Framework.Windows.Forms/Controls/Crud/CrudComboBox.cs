//-----------------------------------------------------------------------
// <copyright file="CrudComboBox.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Text;
using Sphere10.Framework;

namespace Sphere10.Framework.Windows.Forms.Crud {
	public class CrudComboBox : CustomComboBox {
		private readonly CrudGrid _crudGrid;

		[Category("Behavior")]
		public event EventHandlerEx<CrudComboBox, object> EntitySelectionChanged;
		
		public CrudComboBox() {
			base.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			base.DropDownSizeMode = SizeMode.UseCurrentControlSize;
			_crudGrid = new CrudGrid();
			_crudGrid.RightClickForContextMenu = false;
			_crudGrid.LeftClickToDeselect = true;
			_crudGrid.SelectOnMouseUp = true;
			_crudGrid.AutoSelectOnCreate = true;
			_crudGrid.EntitySelected += new EventHandlerEx<CrudGrid, object>(_crudGrid_EntitySelected);
			_crudGrid.EntityDeselected += new EventHandlerEx<CrudGrid, object>(_crudGrid_EntityDeselected);
			_crudGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			base.DropDownControl = _crudGrid;
			AutoHideOnSelect = true;
			SelectedEntity = null;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CrudGrid CrudGrid { get { return _crudGrid; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Func<object, string> DisplayMember { get; set; }

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool AutoHideOnSelect { get; set; }

		[Category("Behavior")]
		[DefaultValue(CrudCapabilities.Default)]
		public CrudCapabilities Capabilities {
			get {
				return _crudGrid.Capabilities;
			}
			set {
				_crudGrid.Capabilities = value;
			}
		}

		[Category("Data")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedEntity {
			get {
				return _crudGrid._selectedEntity;
			}
			set {
				//_selectedEntity = value;
				_crudGrid._selectedEntity = value;
				SetComboText(DetermineDisplayString(value));
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CrudGrid Grid {
			get {
				return _crudGrid;
			}
		}


		public override async void ShowDropDown() {
			try {
				_crudGrid._updating = true;
				base.ShowDropDown();
			} finally {
				_crudGrid._updating = false;				
			}
            await _crudGrid.RefreshGrid();

		}

		public void SetCrudParameters<TEntity>(IEnumerable<ICrudGridColumn> gridBindings, Type entityEditorType, CrudCapabilities capabilities, ICrudDataSource<TEntity> dataSource, Size? size = null, bool autoPageSize = false) {
			try {
				if (entityEditorType != null)
					_crudGrid.SetEntityEditor<TEntity>(entityEditorType);
				_crudGrid.Capabilities = capabilities;//.ClearFlags(CrudCapabilities.CanDelete | CrudCapabilities.CanCreate);
				_crudGrid.GridBindings = gridBindings;
				_crudGrid.SetDataSource(dataSource);
				if (size != null)
					_crudGrid.Size = size.Value;
				_crudGrid.AutoPageSize = autoPageSize;
				_crudGrid.AutoSizeCells();
			} catch (Exception error) {
				ExceptionDialog.Show(this, error);
			}
		}

		protected virtual void OnEntitySelectionChanged(object entity) {
			if (AutoHideOnSelect)
				HideDropDown();
		}

		private void SetComboText(string text) {
			Items.Clear();
			Items.Add(text);
			base.SelectedIndex = 0;
		}

		private string DetermineDisplayString(object entity) {
			if (entity == null)
				return PlaceHolderText;

			if (DisplayMember == null)
				return entity.ToString();

			return DisplayMember(entity);
		}

		private void RaiseEntitySelectionChanged(object selectedEntity) {
			SelectedEntity = selectedEntity;
			OnEntitySelectionChanged(SelectedEntity);
			if (EntitySelectionChanged != null)
				EntitySelectionChanged(this, SelectedEntity);
		}

		void _crudGrid_EntitySelected(CrudGrid arg1, object arg2) {
			this.Text = DetermineDisplayString(arg2);
			RaiseEntitySelectionChanged(arg2);
		}

		void _crudGrid_EntityDeselected(CrudGrid arg1, object arg2) {
			SelectedEntity = null;
			RaiseEntitySelectionChanged(null);
		}
	}
}

//-----------------------------------------------------------------------
// <copyright file="ICrudDataSource.cs" company="Sphere 10 Software">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sphere10.Framework {
	public interface ICrudDataSource<TEntity>  {
		TEntity New();
		void Create(TEntity entity);
		IEnumerable<TEntity> Read(string searchTerm, int pageLength, ref int page, string sortProperty, SortDirection sortDirection, out int totalItems);
		TEntity Refresh(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);
		IEnumerable<string> Validate(TEntity entity, CrudAction action);
	}

}

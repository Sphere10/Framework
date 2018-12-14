//-----------------------------------------------------------------------
// <copyright file="SpannedRangesList.cs" company="Sphere 10 Software">
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

namespace SourceGrid
{
	/// <summary>
	/// A is simple List of Ranges.
	/// Uses simple iterating over list to find
	/// required range
	/// </summary>
public class SpannedRangesList: List<Range>, ISpannedRangesCollection
	{
		public void Update(Range oldRange, Range newRange)
		{
			int index = base.IndexOf(oldRange);
			if (index <0 )
				throw new RangeNotFoundException();
			this[index] = newRange;
		}
		
		public void Redim(int rowCount, int colCount)
		{
			// just do nothing, nothing needed
		}
		
		public new void Remove(Range range)
		{
			int index = base.IndexOf(range);
			if (index < 0)
				throw new RangeNotFoundException();
			base.RemoveAt(index);
		}
		
		
		/// <summary>
		/// Returns first intersecting region
		/// </summary>
		/// <param name="pos"></param>
		public Range? GetFirstIntersectedRange(Position pos)
		{
			for (int i = 0; i < this.Count; i++)
			{
				var range = this[i];
				if (range.Contains(pos))
					return range;
			}
			return null;
		}
		
		public List<Range> GetRanges(Range range)
		{
			var result = new List<Range>();
			for (int i = 0; i < this.Count; i++)
			{
				var r = this[i];
				if (r.Contains(range))
					result.Add(r);
			}
			return result;
		}
		
		public Range? FindRangeWithStart(Position start)
		{
			for (int i = 0; i < this.Count; i++)
			{
				var range = this[i];
				if (range.Start.Equals(start))
					return range;
			}
			return null;
		}
	}
}

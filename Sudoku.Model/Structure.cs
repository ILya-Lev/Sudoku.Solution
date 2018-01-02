using System;
using System.Collections.Generic;

namespace Sudoku.Model
{
	public class Structure
	{
		public IReadOnlyList<Cell> Cells { get; }

		// there could be 12 intersections at most. e.g. ore row intersects with 9 columns and 3 small squares
		public IDictionary<Structure, Cell> Intersections { get; } = new Dictionary<Structure, Cell>(12);

		public Structure(IReadOnlyList<Cell> cells)
		{
			Cells = cells;
		}

		// cannot do it in ctor as created instance of Structure object is required
		public void AddIntersection(Structure with, Cell at)
		{
			if (Intersections.ContainsKey(with))
				throw new InvalidOperationException($"This intersection is already registered");
			Intersections.Add(with, at);
		}
	}
}
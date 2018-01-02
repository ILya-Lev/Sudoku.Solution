using System.Collections.Generic;
using System.Diagnostics;

namespace Sudoku.Model
{
	[DebuggerDisplay("{Cells[0].Value},{Cells[1].Value},{Cells[2].Value},{Cells[3].Value},{Cells[4].Value},{Cells[5].Value},{Cells[6].Value},{Cells[7].Value},{Cells[8].Value}")]
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
			if (!Intersections.ContainsKey(with))
				Intersections.Add(with, at);
		}
	}
}
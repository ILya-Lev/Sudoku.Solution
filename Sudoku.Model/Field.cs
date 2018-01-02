using System.Collections.Generic;
using static Sudoku.Model.Constants;

namespace Sudoku.Model
{
	public class Field
	{
		public List<Structure> Lines { get; set; } = new List<Structure>(Size);
		public List<Structure> Columns { get; set; } = new List<Structure>(Size);
		public List<Structure> Squares { get; set; } = new List<Structure>(Size);
	}
}
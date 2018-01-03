using Sudoku.Model;

namespace Sudoku.Logic.Extensions
{
	public static class CellExtensions
	{
		public static Cell Clone(this Cell source)
		{
			var clone = new Cell(source.Value);
			clone.PossibleValues.Clear();
			clone.PossibleValues.AddRange(source.PossibleValues);
			return clone;
		}
	}
}
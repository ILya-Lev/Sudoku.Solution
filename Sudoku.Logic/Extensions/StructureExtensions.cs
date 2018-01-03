using Sudoku.Model;
using System.Linq;

namespace Sudoku.Logic.Extensions
{
	public static class StructureExtensions
	{
		public static bool IsComplete(this Structure aStructure) => aStructure.Cells.All(c => !c.IsEmpty);

		public static int EmptyCellsAmount(this Structure aStructure) => aStructure.Cells.Count(c => c.IsEmpty);

		public static bool HaveContradictions(this Structure aStructure)
		{
			var filledCells = aStructure.Cells
				.Where(c => !c.IsEmpty)
				.Select(c => c.Value)
				.ToList();

			return filledCells.Distinct().Count() != filledCells.Count;
		}

		public static Structure Clone(this Structure source)
		{
			var clonedCells = source.Cells.Select(c => c.Clone()).ToList();
			var clone = new Structure(clonedCells);
			return clone;
		}
	}
}
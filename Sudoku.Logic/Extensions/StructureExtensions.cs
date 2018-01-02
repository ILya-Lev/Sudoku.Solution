﻿using System.Linq;
using Sudoku.Model;

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

		public static void FilterPossibleValues(this Structure aStructure)
		{
			var presentValues = aStructure.Cells.Where(c => !c.IsEmpty).Select(c => c.Value);
			foreach (var cell in aStructure.Cells)
			{
				if (!cell.IsEmpty)
					continue;

				// presentValues is intentionally evaluated in each loop step as situation may change in intersections parsing
				cell.PossibleValues = cell.PossibleValues.Except(presentValues).ToList();

				if (!cell.IsEmpty)
				{
					aStructure.Intersections
							  .Where(intersection => intersection.Value == cell)
							  .Select(intersection => intersection.Key)
							  .ToList()
							  .ForEach(FilterPossibleValues);
				}
			}
		}
	}
}
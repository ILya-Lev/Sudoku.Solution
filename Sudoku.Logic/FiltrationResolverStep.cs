using Sudoku.Logic.Extensions;
using Sudoku.Model;
using System.Linq;

namespace Sudoku.Logic
{
	public class FiltrationResolverStep : IResolverStep
	{
		public bool TryResolve(Field field)
		{
			int noChangesSteps = 0;
			while (!field.IsCompleted())
			{
				var beforeProcessing = field.EmptyCellsAmount();
				RunPossibleValuesFiltration(field);
				var afterProcessing = field.EmptyCellsAmount();

				if (beforeProcessing == afterProcessing)
				{
					noChangesSteps++;
					if (noChangesSteps == 3)
						return false;
				}
				else
				{
					noChangesSteps = 0;
				}
			}

			return true;
		}

		private static void RunPossibleValuesFiltration(Field aField)
		{
			aField.Lines.ForEach(FilterOutPossibleValues);
			aField.Columns.ForEach(FilterOutPossibleValues);
			aField.Squares.ForEach(FilterOutPossibleValues);
		}

		private static void FilterOutPossibleValues(Structure aStructure)
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
							  .ForEach(FilterOutPossibleValues);
				}
			}
		}
	}
}
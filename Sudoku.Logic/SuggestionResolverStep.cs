using Sudoku.Logic.Extensions;
using Sudoku.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Logic
{
	public class SuggestionResolverStep : IResolverStep
	{
		private readonly FiltrationResolverStep _filtrationStep;

		public SuggestionResolverStep(FiltrationResolverStep filtrationStep)
		{
			_filtrationStep = filtrationStep;
		}

		public bool TryResolve(Field field)
		{
			var digitToSubstitute = DigitWithMinimalUncertainty(field);

			for (var statisticsIndex = 0; statisticsIndex < digitToSubstitute.Count; ++statisticsIndex)
			{
				var statistics = digitToSubstitute[statisticsIndex];
				for (var cellIndex = 0; cellIndex < statistics.Cells.Count; ++cellIndex)
				{
					for (var digitIndex = 0; digitIndex < statistics.Digits.Count; ++digitIndex)
					{
						var clone = field.Clone();
						var cloneStatistics = DigitWithMinimalUncertainty(clone);

						var cell = cloneStatistics[statisticsIndex].Cells[cellIndex];
						var digit = cloneStatistics[statisticsIndex].Digits[digitIndex];

						cell.SetValue(digit);
						if (_filtrationStep.TryResolve(clone) && !clone.HaveContradictions())
						{
							field.CopyFrom(clone);
							return true;
						}
					}
				}
			}

			return false;
		}

		private static List<DigitStatistics> DigitWithMinimalUncertainty(Field field)
		{
			var frequencies = CollectUncertainty(field.Lines)
							  .Concat(CollectUncertainty(field.Columns))
							  .Concat(CollectUncertainty(field.Squares))
							  .OrderBy(statistics => statistics.Digits.Count)
							  .ToList();

			return frequencies;
		}

		private static IEnumerable<DigitStatistics> CollectUncertainty(IEnumerable<Structure> structures)
		{
			return structures
				   .Where(structure => !structure.IsComplete())
				   .OrderBy(structure => structure.EmptyCellsAmount())
				   .Select(structure => structure.Cells.Where(c => c.IsEmpty).ToList())
				   .Select(emptyLineCells => new DigitStatistics
				   {
					   Digits = emptyLineCells.Select(c => c.PossibleValues)
											  .Aggregate((seed, current) => seed.Intersect(current).ToList())
											  .ToList(),
					   Cells = emptyLineCells
				   })
				   .Where(statistics => statistics.Digits.Count != 0);
		}

		private struct DigitStatistics
		{
			public List<int> Digits { get; set; }
			public List<Cell> Cells { get; set; }
		}
	}
}
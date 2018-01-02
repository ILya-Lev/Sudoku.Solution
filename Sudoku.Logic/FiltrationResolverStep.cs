using Sudoku.Logic.Extensions;
using Sudoku.Model;

namespace Sudoku.Logic
{
	public class FiltrationResolverStep : IResolverStep
	{
		public bool IsResolved => EmptyCellsAmount == 0;

		public int EmptyCellsAmount { get; private set; }

		public void TryResolve(Field field)
		{
			EmptyCellsAmount = field.EmptyCellsAmount();
			int noChangesSteps = 0;
			while (!field.IsCompleted())
			{
				field.RunPossibleValuesFiltration();
				var afterProcessing = field.EmptyCellsAmount();

				if (EmptyCellsAmount == afterProcessing)
				{
					noChangesSteps++;
					if (noChangesSteps == 3)
						return;
				}
				else
				{
					EmptyCellsAmount = afterProcessing;
					noChangesSteps = 0;
				}
			}
		}
	}
}
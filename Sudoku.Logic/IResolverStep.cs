using Sudoku.Model;

namespace Sudoku.Logic
{
	public interface IResolverStep
	{
		void TryResolve(Field field);
		bool IsResolved { get; }
		int EmptyCellsAmount { get; }
	}
}
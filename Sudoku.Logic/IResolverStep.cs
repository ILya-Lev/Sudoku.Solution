using Sudoku.Model;

namespace Sudoku.Logic
{
	public interface IResolverStep
	{
		bool TryResolve(Field field);
	}
}
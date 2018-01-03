using Sudoku.Model;
using System.Linq;

namespace Sudoku.Logic.Extensions
{
	public static class FieldExtensions
	{
		public static bool IsCompleted(this Field aField)
		{
			return aField.Lines.All(StructureExtensions.IsComplete);
		}

		public static int EmptyCellsAmount(this Field aField)
		{
			return aField.Lines.Select(line => line.EmptyCellsAmount()).Sum();
		}

		public static bool HaveContradictions(this Field aField)
		{
			return aField.Lines.Any(line => line.HaveContradictions())
				|| aField.Columns.Any(column => column.HaveContradictions())
				|| aField.Squares.Any(square => square.HaveContradictions());
		}

		public static Field Clone(this Field aField)
		{
			var lines = aField.Lines.Select(line => line.Clone()).ToList();

			var copy = FieldFactory.CreateFromLines(lines);

			return copy;
		}

		public static void CopyFrom(this Field target, Field source)
		{
			target.Lines.Clear();
			target.Columns.Clear();
			target.Squares.Clear();

			target.Lines.AddRange(source.Lines);
			target.Columns.AddRange(source.Columns);
			target.Squares.AddRange(source.Squares);
		}

	}
}
using System.Linq;
using Sudoku.Model;

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
			return aField.Lines.Any(line => line.HaveContradictions());
		}

		public static void RunPossibleValuesFiltration(this Field aField)
		{
			aField.Lines.ForEach(line => line.FilterPossibleValues());
			aField.Columns.ForEach(column => column.FilterPossibleValues());
			aField.Squares.ForEach(square => square.FilterPossibleValues());
		}

		public static Field Clone(this Field aField)
		{
			var copy = new Field();
			copy.Lines.AddRange(aField.Lines);
			copy.Columns.AddRange(aField.Columns);
			copy.Squares.AddRange(aField.Squares);

			return copy;
		}
	}
}
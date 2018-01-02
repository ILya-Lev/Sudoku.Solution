using System.Collections.Generic;
using System.Linq;
using static Sudoku.Model.Constants;

namespace Sudoku.Model
{
	public class Field
	{
		public IList<Structure> Lines { get; set; } = new List<Structure>(Size);
		public IList<Structure> Columns { get; set; } = new List<Structure>(Size);
		public IList<Structure> Squares { get; set; } = new List<Structure>(Size);
	}

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

		public static void RunPossibleValuesFiltration(this Field aField)
		{
			foreach (var line in aField.Lines)
			{
				line.FilterPossibleValues();
			}
		}
	}

	public class Resolver
	{
		public bool Resolve(Field field)
		{
			while (!field.IsCompleted())
			{
				var beforeProcessing = field.EmptyCellsAmount();
				field.RunPossibleValuesFiltration();
				var afterProcessing = field.EmptyCellsAmount();

				if (beforeProcessing == afterProcessing)
					return false;
			}

			return true;
		}
	}
}
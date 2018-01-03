using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sudoku.Model
{
	[DebuggerDisplay("{Value}")]
	public class Cell
	{
		public int Value { get; private set; }
		public bool IsEmpty => Value == 0;

		private List<int> _possibleValues = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToList();
		public List<int> PossibleValues
		{
			get { return _possibleValues; }
			set
			{
				_possibleValues = value;
				if (_possibleValues.Count == 1)
					SetValue(_possibleValues[0]);
			}
		}

		public Cell(int initialValue)
		{
			SetValue(initialValue);
		}

		public void SetValue(int aValue)
		{
			if (aValue == 0)
				return;

			Value = aValue;
			_possibleValues.Clear();
		}
	}
}

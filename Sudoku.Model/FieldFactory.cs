using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Sudoku.Model.Constants;

namespace Sudoku.Model
{
	public static class FieldFactory
	{
		public static Field CreateFromFile(string fullFilePath)
		{
			var lines = ReadLinesFromFile(fullFilePath);

			var field = new Field
			{
				Lines = lines,
				Columns = GenerateColumnsFromLines(lines).ToList(),
				Squares = GenerateSquaresFromLines(lines).ToList()
			};

			FulfillIntersectionsOf(field.Columns, field.Squares);

			return field;
		}

		private static List<Structure> ReadLinesFromFile(string fullFilePath)
		{
			if (!File.Exists(fullFilePath))
				throw new
					FileNotFoundException($"Cannot find file at {fullFilePath}; working directory {Environment.CurrentDirectory}");

			var lines = File
						.ReadLines(fullFilePath)
						.Where(row => !string.IsNullOrWhiteSpace(row))
						.Select(row => row.ToCharArray().Where(char.IsDigit).Select(ch => new string(ch, 1)).ToList())
						.Select(rowPieces => rowPieces.Select(piece => new Cell(int.Parse(piece))).ToList())
						.Select(rowCells => new Structure(rowCells))
						.ToList();
			return lines;
		}

		private static IEnumerable<Structure> GenerateColumnsFromLines(List<Structure> lines)
		{
			for (int columnIndex = 0; columnIndex < Size; columnIndex++)
			{
				var columnCells = lines.Select(line => line.Cells[columnIndex]).ToList();
				var column = new Structure(columnCells);

				lines.ForEach(line =>
				{
					line.AddIntersection(column, line.Cells[columnIndex]);
					column.AddIntersection(line, line.Cells[columnIndex]);
				});

				yield return column;
			}
		}

		private static IEnumerable<Structure> GenerateSquaresFromLines(List<Structure> lines)
		{
			for (int squareIndex = 0; squareIndex < Size; squareIndex++)
			{
				var squareCells = lines.Where(IsLineForTheSquare(squareIndex))
									   .SelectMany(line => line.Cells.Where(IsCellALineSquareCross(squareIndex)))
									   .ToList();
				var square = new Structure(squareCells);

				foreach (var line in lines.Where(IsLineForTheSquare(squareIndex)))
				{
					line.Cells
						.Where(IsCellALineSquareCross(squareIndex))
						.ToList()
						.ForEach(cell =>
						{
							line.AddIntersection(square, cell);
							square.AddIntersection(line, cell);
						});
				}

				yield return square;
			}
		}

		private static void FulfillIntersectionsOf(List<Structure> columns, List<Structure> squares)
		{
			for (int squareIndex = 0; squareIndex < Size; squareIndex++)
			{
				foreach (var column in columns.Where(IsColumnForTheSquare(squareIndex)))
				{
					column.Cells
						.Where(IsCellAColumnSquareCross(squareIndex))
						.ToList()
						.ForEach(cell =>
						{
							column.AddIntersection(squares[squareIndex], cell);
							squares[squareIndex].AddIntersection(column, cell);
						});
				}
			}
		}

		private static Func<Structure, int, bool> IsColumnForTheSquare(int squareIndex)
		{
			return (column, idx) => idx / Divisor == squareIndex % Divisor;
		}

		private static Func<Structure, int, bool> IsLineForTheSquare(int squareIndex)
		{
			return (line, idx) => idx / Divisor == squareIndex / Divisor;
		}

		private static Func<Cell, int, bool> IsCellALineSquareCross(int squareIndex)
		{
			return (c, idx) =>
			{
				// a vertical stripe of width = Divisor columns
				var stripePosition = Divisor * (squareIndex % Divisor);
				var stripe = Enumerable.Range(stripePosition, Divisor);
				return stripe.Contains(idx);
			};
		}

		private static Func<Cell, int, bool> IsCellAColumnSquareCross(int squareIndex)
		{
			return (c, idx) =>
			{
				// a horizontal stripe of width = Divisor lines
				var stripePosition = Divisor * (squareIndex / Divisor);
				var stripe = Enumerable.Range(stripePosition, Divisor);
				return stripe.Contains(idx);
			};
		}
	}
}
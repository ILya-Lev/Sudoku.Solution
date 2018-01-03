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

			return CreateFromLines(lines);
		}

		public static Field CreateFromLines(List<Structure> lines)
		{
			var field = new Field
			{
				Lines = lines,
				Columns = GenerateColumnsFromLines(lines).ToList(),
				Squares = GenerateSquaresFromLines(lines).ToList()
			};

			IntersectionsCreator.AddLineWithColumnIntersections(lines, field.Columns);
			IntersectionsCreator.AddLineWithSquareIntersections(lines, field.Squares);
			IntersectionsCreator.AddColumnWithSquareIntersections(field.Columns, field.Squares);

			return field;
		}

		private static List<Structure> ReadLinesFromFile(string fullFilePath)
		{
			if (!File.Exists(fullFilePath))
			{
				throw new FileNotFoundException($"Cannot find file at {fullFilePath};"
											  + $" working directory {Environment.CurrentDirectory}");
			}

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

				yield return column;
			}
		}

		private static IEnumerable<Structure> GenerateSquaresFromLines(List<Structure> lines)
		{
			for (int squareIndex = 0; squareIndex < Size; squareIndex++)
			{
				var squareCells = lines
					.Where(IntersectionsCreator.IsLineForTheSquare(squareIndex))
					.SelectMany(line => line.Cells
									  .Where(IntersectionsCreator.IsCellALineSquareCross(squareIndex)))
					.ToList();
				var square = new Structure(squareCells);

				yield return square;
			}
		}
	}
}
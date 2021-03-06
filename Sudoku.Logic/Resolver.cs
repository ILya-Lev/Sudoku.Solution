﻿using Sudoku.Logic.Extensions;
using Sudoku.Model;
using System.Collections.Generic;

namespace Sudoku.Logic
{
	public class Resolver
	{
		private readonly List<IResolverStep> _steps;

		public Resolver(List<IResolverStep> steps)
		{
			_steps = steps;
		}

		public Field FillIn(Field field)
		{
			var clonedField = field.Clone();
			foreach (var step in _steps)
			{
				if (step.TryResolve(clonedField))
					return clonedField;
			}

			return clonedField;
		}
	}
}

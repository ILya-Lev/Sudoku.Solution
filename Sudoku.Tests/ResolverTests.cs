using FluentAssertions;
using Sudoku.Model;
using System;
using System.IO;
using Xunit;

namespace Sudoku.Tests
{
	public class ResolverTests
	{
		[InlineData(@"..\..\Samples\BasicLevel\01.txt")]
		//[InlineData(@"..\..\Samples\BasicLevel\02.txt")]
		//[InlineData(@"..\..\Samples\BasicLevel\03.txt")]
		//[InlineData(@"..\..\Samples\BasicLevel\04.txt")]
		//[InlineData(@"..\..\Samples\BasicLevel\05.txt")]
		[Theory]
		public void Resolve_BasicLevelProblems_ShouldSolve(string path)
		{
			var field = FieldFactory.CreateFromFile(Path.Combine(Environment.CurrentDirectory, path));
			var resolver = new Resolver();

			var isResolved = resolver.Resolve(field);

			isResolved.Should().BeTrue();
		}
	}
}

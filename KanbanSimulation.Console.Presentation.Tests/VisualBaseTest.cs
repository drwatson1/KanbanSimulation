using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.Console.View.Tests
{
	class ConcreateVisualBase:	VisualBase
	{
		public ConcreateVisualBase(int width, int height)
			:	base(width, height)
		{

		}

		public void SetWidth(int width)
		{
			Width = width;
		}

		public void SetHeight(int height)
		{
			Height = height;
		}

		public override void Render()
		{
			throw new NotImplementedException();
		}
	}

	[TestClass]
	public class VisualBaseTest
	{
		[TestMethod]
		public void InitialWidthMustBeZeroOrMore()
		{
			
			Action a = () => new ConcreateVisualBase(-1, 0);

			a.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void InitialHeightMustBeZeroOrMore()
		{
			Action a = () => new ConcreateVisualBase(0, -1);

			a.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void WidthMustBeZeroOrMore()
		{
			var v = new ConcreateVisualBase(0, 0);
			Action a = () => v.SetWidth(-1);

			a.ShouldThrow<ArgumentException>();
			v.Width.Should().Be(0);
		}

		[TestMethod]
		public void HeightMustBeZeroOrMore()
		{
			var v = new ConcreateVisualBase(0, 0);
			Action a = () => v.SetHeight(-1);

			a.ShouldThrow<ArgumentException>();
			v.Height.Should().Be(0);
		}
	}
}

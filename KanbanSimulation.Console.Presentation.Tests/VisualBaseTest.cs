using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.Console.View.Tests
{
	[TestClass]
	public class VisualBaseTest
	{
		[TestMethod]
		public void InitialWidthMustBeZeroOrGreater()
		{
			
			Action a = () => new VisualBaseMock(-1, 0);

			a.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void InitialHeightMustBeZeroOrGreater()
		{
			Action a = () => new VisualBaseMock(0, -1);

			a.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void WidthMustBeZeroOrGreater()
		{
			var v = new VisualBaseMock(0, 0);
			Action a = () => v.SetWidth(-1);

			a.ShouldThrow<ArgumentException>();
			v.Width.Should().Be(0);
		}

		[TestMethod]
		public void HeightMustBeZeroOrGreater()
		{
			var v = new VisualBaseMock(0, 0);
			Action a = () => v.SetHeight(-1);

			a.ShouldThrow<ArgumentException>();
			v.Height.Should().Be(0);
		}


		[TestMethod]
		public void VisualShouldBeVisibleByDefaultAfterCreation()
		{
			var v = new VisualBaseMock(10, 10);

			v.Visible.Should().Be(true);
		}

		[TestMethod]
		public void WidthAndHeightInvisibleVisualShouldBeZero()
		{
			var v = new VisualBaseMock(10, 10);

			v.Visible = false;

			v.Width.Should().Be(0);
			v.Height.Should().Be(0);
		}
	}
}

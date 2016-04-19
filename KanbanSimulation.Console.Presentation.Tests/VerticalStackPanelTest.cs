using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanSimulation.Console.View.Tests
{
	[TestClass]
	public class VerticalStackPanelTest
	{
		[TestMethod]
		public void InitialWidthHeightMustBeZero()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);

			p.Width.Should().Be(0);
			p.Height.Should().Be(0);
		}

		[TestMethod]
		public void MustCorrectlyShiftChildPosition()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);
			p.Position = new Position(10, 20);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			p.Arrange();

			txt1.Position.Left.Should().Be(10);
			txt1.Position.Top.Should().Be(20);

			txt2.Position.Left.Should().Be(10);
			txt2.Position.Top.Should().Be(21);
		}

		[TestMethod]
		public void WidthMustBeMaxValueOfChilds()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);
			p.Arrange();

			p.Width.Should().Be(5);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			p.Arrange();

			p.Width.Should().Be(5);
		}

		[TestMethod]
		public void HeightMustCorrectlyIncreasedAfterInsertingSomeTextBlocks()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			p.Arrange();

			p.Height.Should().Be(2);
		}

		[TestMethod]
		public void RenderMustBeCalledForAllChilds()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);

			var r = new RendererMock();
			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			p.Arrange();

			p.Render(r);
			r.SetPositionCount.Should().Be(2);
			r.WriteCount.Should().Be(2);
		}

		[TestMethod]
		public void ArrangeShouldIgnoreInvisibleChilds()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			var txt3 = new TextBox(4, 1, "abcde");
			p.AddChild(txt3);

			p.Arrange();

			txt2.Visible = false;

			p.Arrange();

			p.Height.Should().Be(2);
		}

		[TestMethod]
		public void InvisibleChildsShouldNotBeRendered()
		{
			var p = new StackPanel(StackPanelOrientation.Vertical);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new VisualBaseMock(10, 10);
			p.AddChild(txt2);

			p.Arrange();

			txt2.Visible = false;

			p.Arrange();

			var r = new RendererMock();
			p.Render(r);

			txt2.RenderCallCount.Should().Be(0);
		}
	}
}
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanSimulation.Console.View.Tests
{
	[TestClass]
	public class HorizontalStackPanelTest
	{
		[TestMethod]
		public void InitialWidthHeightMustBeZero()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

			p.Width.Should().Be(0);
			p.Height.Should().Be(0);
		}

		[TestMethod]
		public void MustCorrectlyShiftChildPosition()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);
			p.Position = new Position(10, 20);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			p.Arrange();

			txt1.Position.Left.Should().Be(10);
			txt1.Position.Top.Should().Be(20);

			txt2.Position.Left.Should().Be(15);
			txt2.Position.Top.Should().Be(20);
		}

		[TestMethod]
		public void MustIncreaseWidthAfterInsertingTextBlock()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);
			p.Arrange();

			p.Width.Should().Be(5);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);
			p.Arrange();

			p.Width.Should().Be(8);
		}

		[TestMethod]
		public void HeightMustBeEqualsOneAfterInsertingSomeTextBlocks()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var txt2 = new TextBox(3, 1, "abcde");
			p.AddChild(txt2);

			p.Arrange();

			p.Height.Should().Be(1);
		}

		[TestMethod]
		public void RenderMustBeCalledForAllChilds()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

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
		public void SizeMustCorrectForChildStackPanel()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

			var txt1 = new TextBox(5, 1, "abc");
			p.AddChild(txt1);

			var p1 = new StackPanel(StackPanelOrientation.Vertical);

			p.AddChild(p1);

			var txt2 = new TextBox(3, 1, "abc");
			p1.AddChild(txt2);

			var p2 = new StackPanel(StackPanelOrientation.Horizontal);

			p1.AddChild(p2);

			var txt3 = new TextBox(4, 1, "abc");
			p2.AddChild(txt3);

			var txt4 = new TextBox(5, 1, "abc");
			p2.AddChild(txt4);

			p.Arrange();

			p2.Width.Should().Be(9);
			p2.Height.Should().Be(1);

			p1.Width.Should().Be(9);
			p1.Height.Should().Be(2);

			p.Width.Should().Be(14);
			p.Height.Should().Be(2);
		}
	}
}
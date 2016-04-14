using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.Console.View.Tests
{
	[TestClass]
	public class TextBoxTest
	{
		[TestMethod]
		public void RenderMustRightPadStringToTextBlockWidth()
		{
			string sampleText = "abc";

			var r = new RendererMock();
			var txt = new TextBox(5, 1, sampleText);
			txt.Position = new Position(1, 2);

			txt.Render(r);

			r.Text.ShouldBeEquivalentTo("abc  ");
		}

		[TestMethod]
		public void RenderMustTruncateStringToTextBlockWidth()
		{
			string sampleText = "abcde";

			var r = new RendererMock();
			var txt = new TextBox(3, 1, sampleText);
			txt.Position = new Position(1, 2);

			txt.Render(r);

			r.Position.Left.Should().Be(1);
			r.Position.Top.Should().Be(2);
			r.Text.ShouldBeEquivalentTo("abc");
		}

		[TestMethod]
		public void RenderMustSetPosition()
		{
			string sampleText = "abc";

			var r = new RendererMock();
			var txt = new TextBox(5, 1, sampleText);
			txt.Position = new Position(1, 2);

			txt.Render(r);

			r.Position.Left.Should().Be(1);
			r.Position.Top.Should().Be(2);
		}

		[TestMethod]
		public void MustRenderBlankStringIfDatasourceIsNull()
		{
			var r = new RendererMock();
			var txt = new TextBox(5, 1);

			txt.Render(r);

			r.Text.Should().BeEquivalentTo("     ");
		}
	}
}

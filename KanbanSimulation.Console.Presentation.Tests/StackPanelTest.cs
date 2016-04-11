using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.Console.ViewModel.Tests
{
	[TestClass]
	public class StackPanelTest
	{
		[TestMethod]
		public void HorizontalStackPanelMustIncreaseWidthAfterInsertingTextBlock()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);
			p.AddChild(new TextBlock(10));

			p.Width.Should().Be(10);
		}
	}
}

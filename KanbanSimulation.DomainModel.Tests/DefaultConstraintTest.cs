using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class DefaultConstraintTest
	{
		[TestMethod]
		public void ShouldNotViolatedAlways()
		{
			var c = new DefaultConstraint();

			c.CanTake(1).Should().Be(true);
		}
	}
}
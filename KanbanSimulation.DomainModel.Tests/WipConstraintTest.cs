using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WipConstraintTest
	{
		[TestMethod]
		public void ShouldBeViolatedIfTryToTakeTwoWorkItemsForLimitOne()
		{
			var op = new Operation();

			var c = new WipConstraint(op, 1);

			c.CanTake(2).Should().Be(false);
		}

		[TestMethod]
		public void ShouldNotBeViolatedIfTryToTakeOneWorkItemForLimitOne()
		{
			var op = new Operation();

			var c = new WipConstraint(op, 1);

			c.CanTake(1).Should().Be(true);
		}
	}
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WorkProcessTest
	{
		private WorkProcess CreateWorkProcess()
		{
			WorkProcess wp = new WorkProcess()
				.AddOperation(new Operation())
				.AddOperation(new Operation(2))
				.AddOperation(new Operation());

			return wp;
		}

		[TestMethod]
		public void WorkProcessCanReturnAddedOperations()
		{
			var wp = CreateWorkProcess();

			wp.Operations.Count.Should().Be(3);
		}

		[TestMethod]
		public void InputQueueContainsAllPushedWorkItems()
		{
			var wp = CreateWorkProcess();

			wp
				.Push(new WorkItem(1))
				.Push(new WorkItem(2));

			wp.InputQueue.Count.Should().Be(2);
		}
	}
}

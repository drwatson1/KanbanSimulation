using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WorkProcessPullStrategyTest
	{
		[TestMethod]
		public void OperationShouldPushToDoneQueue()
		{
			var s = new WorkProcessPullStrategy();

			var op = new Operation();
			var outputQueue = new WorkItemQueue();
			s.ConfigureOutputQueue(op, outputQueue);

			op.Push(new WorkItem());

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			outputQueue.Count.Should().Be(0);
			op.Done.Count.Should().Be(1);
		}

		[TestMethod]
		public void OperationShouldPullFromInputQueue()
		{
			var s = new WorkProcessPullStrategy();

			var wi1 = new WorkItem();
			var wi2 = new WorkItem();

			var inputQueue = new WorkItemQueue();

			var op = new Operation();

			s.ConfigureInputQueue(op, inputQueue);

			inputQueue.Push(wi1);
			op.TakeNewWorkItems();

			inputQueue.Count.Should().Be(0);
		}
	}
}

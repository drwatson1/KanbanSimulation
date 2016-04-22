using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WorkProcessPushStrategyTest
	{
		[TestMethod]
		public void ShouldPushToOutputQueue()
		{
			var s = new WorkProcessPushStrategy();

			var op = new Operation();
			var outputQueue = new WorkItemQueue();
			s.ConfigureOutputQueue(op, outputQueue);

			op.Push(new WorkItem());

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			outputQueue.Count.Should().Be(1);
			op.Done.Count.Should().Be(0);
		}

		[TestMethod]
		public void ShouldPullFromInProgressQueue()
		{
			var s = new WorkProcessPushStrategy();

			var inputQueue = new WorkItemQueue();
			var op = new Operation();

			op.Push(new WorkItem());
			inputQueue.Push(new WorkItem());

			s.ConfigureInputQueue(op, inputQueue);

			inputQueue.Count.Should().Be(1);
		}
	}
}

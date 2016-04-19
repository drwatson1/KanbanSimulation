using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WorkProcessPullStrategyTest
	{
		[TestMethod]
		public void PullStrategyShouldPushToOutputQueue()
		{
			var s = new WorkProcessPullStrategy();

			var wi = new WorkItem();

			var doneQueue = new WorkItemQueue();
			var outputQueue = new WorkItemQueue();
			s.Push(wi, doneQueue, outputQueue);

			outputQueue.Count.Should().Be(0);
			doneQueue.Count.Should().Be(1);
		}

		[TestMethod]
		public void PullStrategyShouldPullFromInputQueue()
		{
			var s = new WorkProcessPullStrategy();

			var wi1 = new WorkItem();
			var wi2 = new WorkItem();

			var inputQueue = new WorkItemQueue();
			var inProgressQueue = new WorkItemQueue();

			inputQueue.Push(wi1);
			inProgressQueue.Push(wi2);

			s.Pull(inputQueue, inProgressQueue);

			inputQueue.Count.Should().Be(0);
			inProgressQueue.Count.Should().Be(2);
		}
	}
}

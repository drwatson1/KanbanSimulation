using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using KanbanSimulation.DomainModel.Events;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WorkItemQueueTest
	{
		[TestMethod]
		public void InitialQueueIsEmpty()
		{
			WorkItemQueue q = new WorkItemQueue();

			q.Empty.Should().Be(true);
			q.Count.Should().Be(0);
		}

		[TestMethod]
		public void CantPullFromEmptyQueue()
		{
			WorkItemQueue q = new WorkItemQueue();

			Action action = () => q.Pull();

			action.ShouldThrow<QueueIsEmptyException>();

		}

		[TestMethod]
		public void PushMakeQueueNotEmptyAndValidCount()
		{
			WorkItemQueue q = new WorkItemQueue();

			q.Push(new WorkItem(3));

			q.Empty.Should().Be(false);
			q.Count.Should().Be(1);
		}

		[TestMethod]
		public void PullMakeQueueCountDecrement()
		{
			WorkItemQueue q = new WorkItemQueue();

			q.Push(new WorkItem(3));
			q.Pull();

			q.Count.Should().Be(0);
			q.Empty.Should().Be(true);
		}

		[TestMethod]
		public void CanPullFirstItem()
		{
			WorkItemQueue q = new WorkItemQueue();

			var wi0 = new WorkItem(3);
			q.Push(wi0);
			q.Push(new WorkItem(4));

			var wi1 = q.Pull();

			wi1.ShouldBeEquivalentTo(wi0);
		}

		[TestMethod]
		public void TopMustBeFirstPushedItem()
		{
			WorkItemQueue q = new WorkItemQueue();

			var wi0 = new WorkItem(3);
			q.Push(wi0);
			q.Push(new WorkItem(4));

			q.Top.ShouldBeEquivalentTo(wi0);
		}

		[TestMethod]
		public void TickMustIncrementLeadTimeForAllWorkItems()
		{
			WorkItemQueue q = new WorkItemQueue();

			var wi0 = new WorkItem(3);
			q.Push(wi0);
			var wi1 = new WorkItem(4);
			q.Push(wi1);

			q.Tick();

			wi0.LeadTime.Should().Be(1);
			wi1.LeadTime.Should().Be(1);
		}

		[TestMethod]
		public void QueueMustRaiseEventAfterPush()
		{
			WorkItemQueue q = new WorkItemQueue();

			var wi0 = new WorkItem(3);
			q.Push(wi0);

			q.DomainEvents.Should().ContainSingle();

			var ev = q.DomainEvents[0] as WorkItemQueueChangedEvent;
			ev.Should().NotBeNull();

			ev.Operation.Should().Be(WorkItemQueueChangedEvent.QueueOperation.Push);
			ev.Sender.ShouldBeEquivalentTo(q);
			ev.WorkItem.ShouldBeEquivalentTo(wi0);
		}

		[TestMethod]
		public void QueueMustRaiseEventAfterPull()
		{
			WorkItemQueue q = new WorkItemQueue();

			var wi0 = new WorkItem(3);
			q.Push(wi0);
			q.ClearEvents();
			q.Pull();

			q.DomainEvents.Should().ContainSingle();

			var ev = q.DomainEvents[0] as WorkItemQueueChangedEvent;
			ev.Should().NotBeNull();

			ev.Operation.Should().Be(WorkItemQueueChangedEvent.QueueOperation.Pull);
			ev.Sender.ShouldBeEquivalentTo(q);
			ev.WorkItem.ShouldBeEquivalentTo(wi0);
		}
	}
}

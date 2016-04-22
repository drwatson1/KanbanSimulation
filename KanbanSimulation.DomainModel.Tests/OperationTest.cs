﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using KanbanSimulation.DomainModel.Events;
using System.Linq;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class OperationTest
	{
		[TestMethod]
		public void NotThrowExceptionInTakeNewWorkItemIfNothingToDo()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			Action a = op.TakeNewWorkItems;

			a.ShouldNotThrow();
		}

		[TestMethod]
		public void WipIncrementedByOneAfterTakeNewWorkItem()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			input.Push(new WorkItem(1));

			op.TakeNewWorkItems();

			op.WorkInProgress.Should().Be(1);
		}

		[TestMethod]
		public void LeadTimeMustNotIncrementedAfterTakeNewWorkItem()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);

			op.TakeNewWorkItems();

			wi.LeadTime.Should().Be(0);
		}

		[TestMethod]
		public void CurrentProgressMustBeZeroForNewWorkItem()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);
			input.Top.Tick();

			op.TakeNewWorkItems();

			wi.LeadTime.Should().Be(1);
			wi.CurrentOperationProgress.Should().Be(0);
		}

		[TestMethod]
		public void LeadTimeMustBeIncremetedAfterDoWork()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);
			input.Top.Tick();

			op.TakeNewWorkItems();
			op.DoWork();

			wi.LeadTime.Should().Be(2);
			wi.CurrentOperationProgress.Should().Be(1);
		}

		[TestMethod]
		public void WorkItemsNotMovedAfterDoWork()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);
			input.Top.Tick();

			op.TakeNewWorkItems();
			op.DoWork();

			op.WorkInProgress.Should().Be(1);
			op.InProgress.Count.Should().Be(0);
			op.Done.Count.Should().Be(0);
			output.Count.Should().Be(0);
		}

		[TestMethod]
		public void CompletedOperationMovedToOutputForPushStrategy()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);
			input.Top.Tick();

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			op.InProgress.Count.Should().Be(0);
			output.Count.Should().Be(1);
			op.Done.Count.Should().Be(0);
		}

		[TestMethod]
		public void CompletedOperationDoesNotChangeLeadTimeForPushStrategy()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);
			input.Top.Tick();

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			wi.LeadTime.Should().Be(2);

			WorkItem wi1 = new WorkItem(1);
			input.Push(wi1);

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			wi.LeadTime.Should().Be(2);
		}

		[TestMethod]
		public void NewOperationStartsAfterTakeNewWorkItemFromInputQueue()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			WorkItem wi = new WorkItem(1);
			input.Push(wi);
			input.Top.Tick();

			op.TakeNewWorkItems();
			wi.CurrentOperationProgress.Should().Be(0);
			wi.LeadTime.Should().Be(1);
		}
		
		[TestMethod]
		public void NewOperationStartsAfterTakeNewWorkItemFromInProgress()
		{
			Operation op = new Operation();

			WorkItem wi = new WorkItem(1);
			op.Push(wi);
			wi.Tick();
			op.TakeNewWorkItems();

			wi.CurrentOperationProgress.Should().Be(0);
			wi.LeadTime.Should().Be(1);
		}

		[TestMethod]
		public void SecondWorkItemStartNewOperationOnlyAfterCompletePrevious()
		{
			var outputQueue = new WorkItemQueue(); 
			Operation op = new Operation();
			op.OutputQueue = outputQueue;

			WorkItem wi1 = new WorkItem(1);
			op.Push(wi1);
			WorkItem wi2 = new WorkItem(2);
			op.Push(wi2);

			// Первый WI
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			// Второй WI
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			outputQueue.Count.Should().Be(2);

			wi2.LeadTime.Should().Be(2);
			wi2.CurrentOperationProgress.Should().Be(1);
		}

		[TestMethod]
		public void SecondWorkItemInProgressHasLeadTimeEqualsTwo()
		{
			// В InProgress у нас два WI. После завершения работы над обоими - у первого LeadTime будет 1, у второго - 2

			Operation op = new Operation();
			op.OutputQueue = new WorkItemQueue();


			WorkItem wi1 = new WorkItem(1);
			op.Push(wi1);
			WorkItem wi2 = new WorkItem(2);
			op.Push(wi2);

			// Первый WI
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			// Второй WI
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			wi1.LeadTime.Should().Be(1);
			wi2.LeadTime.Should().Be(2);
		}

		[TestMethod]
		public void WipIncludeInProgressAndDone()
		{
			Operation op = new Operation();

			WorkItem wi1 = new WorkItem(1);
			op.Push(wi1);
			WorkItem wi2 = new WorkItem(2);
			op.Push(wi2);

			// Первый WI
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			op.WorkInProgress.Should().Be(2);
		}

		[TestMethod]
		[Ignore]
		public void TakeNewWorkItemsFromOuterQueueMustRaisePushAndWipChangedEvent()
		{
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

			input.Push(new WorkItem(1));
			op.ClearEvents();

			op.TakeNewWorkItems();

			op.DomainEvents.Should().HaveCount(2);
			op.DomainEvents.Should().Contain(x => x is WorkInProgressChangedEvent);
			op.DomainEvents.Should().Contain(x => x is WorkItemQueueChangedEvent);
			(op.DomainEvents.Single(x => x is WorkItemQueueChangedEvent) as WorkItemQueueChangedEvent).Operation.Should().Be(WorkItemQueueChangedEvent.QueueOperation.Push);
		}

		[TestMethod]
		public void PushWorkItemToOperationMustRaisePushAndWipChangedEvents()
		{
			Operation op = new Operation();
			op.Push(new WorkItem(1));

			op.DomainEvents.Should().HaveCount(2);
			op.DomainEvents.Should().Contain(x => x is WorkInProgressChangedEvent);
			op.DomainEvents.Should().Contain(x => x is WorkItemQueueChangedEvent);
			(op.DomainEvents.Single(x => x is WorkItemQueueChangedEvent) as WorkItemQueueChangedEvent).Operation.Should().Be(WorkItemQueueChangedEvent.QueueOperation.Push);

		}

		[TestMethod]
		public void DoWorkMustNotRaiseEvents()
		{
			Operation op = new Operation();

			op.Push(new WorkItem(1));
			op.TakeNewWorkItems();
			op.ClearEvents();
			op.DoWork();

			op.DomainEvents.Should().HaveCount(0);
		}

		[TestMethod]
		public void TakeNewWorkItemsMustRaiseEventsWithoutStrategy()
		{
			Operation op = new Operation();

			op.Push(new WorkItem(1));
			op.ClearEvents();
			op.TakeNewWorkItems();

			op.DomainEvents.Should().ContainSingle(x => (x as WorkItemQueueChangedEvent) != null && (x as WorkItemQueueChangedEvent).Operation == WorkItemQueueChangedEvent.QueueOperation.Pull);
		}

		[TestMethod]
		[Ignore]
		public void TakeNewWorkItemsMustNotRaiseEventsWithPullStrategy()
		{
			Operation op = new Operation();

			var s = new WorkProcessPullStrategy();

			var inputQueue = new WorkItemQueue();
			s.ConfigureInputQueue(op, inputQueue);
			inputQueue.Push(new WorkItem(1));

			op.ClearEvents();
			op.TakeNewWorkItems();

			op.DomainEvents.Should().ContainSingle(x => (x as WorkInProgressChangedEvent) != null);
		}

		[TestMethod]
		public void MoveCompletedWorkItemsMustRaiseTwoEvents()
		{
			Operation op = new Operation();

			op.Push(new WorkItem(1));
			op.TakeNewWorkItems();
			op.DoWork();
			op.ClearEvents();
			op.MoveCompletedWorkItems();

			op.DomainEvents.Should().HaveCount(2);
			op.WorkInProgress.Should().Be(1);
		}

		[TestMethod]
		public void MoveCompletedWorkItemsToOuterQueueMustChangeWipForPushStrategy()
		{
			Operation op = new Operation();
			op.OutputQueue = new WorkItemQueue();

			op.Push(new WorkItem(1));
			op.TakeNewWorkItems();
			op.DoWork();
			op.ClearEvents();
			op.MoveCompletedWorkItems();

			op.WorkInProgress.Should().Be(0);
		}

		[TestMethod]
		public void ComplexityEqualsTwoShouldRequireFourDoWorkForTwoWorkItems()
		{
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(2);
			op.OutputQueue = output;

			op.Push(new WorkItem(1));
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			op.Push(new WorkItem(2));
			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			output.Count.Should().Be(1);
			output[0].Id.Should().Be(1);

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			output.Count.Should().Be(1);

			op.TakeNewWorkItems();
			op.DoWork();
			op.MoveCompletedWorkItems();

			output.Count.Should().Be(2);
		}
	}
}

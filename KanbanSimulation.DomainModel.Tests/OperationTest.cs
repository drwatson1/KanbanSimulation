using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

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

			Action a = () => op.TakeNewWorkItems();

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

			op.InProgress.Count.Should().Be(1);
			op.Done.Count.Should().Be(0);
		}

		[TestMethod]
		public void CompletedOperationMovedToOutput()
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
		}

		[TestMethod]
		public void CompletedOperationDoesNotChangeLeadTime()
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
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

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
			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

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

			output.Count.Should().Be(2);

			wi2.LeadTime.Should().Be(2);
			wi2.CurrentOperationProgress.Should().Be(1);
		}

		[TestMethod]
		public void SecondWorkItemInProgressHasLeadTimeEqualsTwo()
		{
			// В InProgress у нас два WI. После завершения работы над обоими - у первого LeadTime будет 1, у второго - 2

			WorkItemQueue input = new WorkItemQueue();
			WorkItemQueue output = new WorkItemQueue();

			Operation op = new Operation(input, output);

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
	}
}

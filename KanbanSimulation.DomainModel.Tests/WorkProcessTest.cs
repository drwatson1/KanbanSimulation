using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;
using KanbanSimulation.DomainModel.Events;
using KanbanSimulation.DomainModel.Interfaces;

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

		private WorkProcess CreateWorkProcess(IWorkProcessStrategy strategy)
		{
			WorkProcess wp = new WorkProcess(strategy)
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
		public void InputQueueContainsAllPushedWorkItemsForPullStrategy()
		{
			var wp = CreateWorkProcess(new WorkProcessPullStrategy());

			wp
				.Push(new WorkItem(1))
				.Push(new WorkItem(2));

			wp.InputQueue.Count.Should().Be(2);
			wp.WorkInProgress.Should().Be(0);
		}

		[TestMethod]
		public void WorkItemMustReachFirstOperationAfterFirstTick()
		{
			var wp = CreateWorkProcess();

			wp
				.Push(new WorkItem(1));

			wp.Tick();

			wp.Operations[0].WorkInProgress.Should().Be(1);
			wp.Operations[1].WorkInProgress.Should().Be(0);
		}

		[TestMethod]
		public void WorkItemMustReachSecondOperationAfterCompletedFirst()
		{
			var wp = CreateWorkProcess();

			wp
				.Push(new WorkItem(1));

			wp.Tick(4);

			wp.Operations[0].WorkInProgress.Should().Be(0);
			wp.Operations[1].WorkInProgress.Should().Be(1);
		}

		[TestMethod]
		public void WorkItemMustReachDone()
		{
			var wp = CreateWorkProcess();

			wp.Push(new WorkItem(1));

			wp.Tick(12);

			wp.Done.Count.Should().Be(1);
		}

		[TestMethod]
		public void AllPushedWorkItemsMustReachDone()
		{
			var wp = CreateWorkProcess();

			wp.Push(new WorkItem(1));
			wp.Push(new WorkItem(2));
			wp.Push(new WorkItem(3));

			wp.Tick(24);

			wp.Done.Count.Should().Be(3);
		}

		[TestMethod]
		public void DoneWorkItemMustHaveValidLeadTime()
		{
			var wp = CreateWorkProcess();

			var wi = new WorkItem(1);
			wp.Push(wi);

			wp.Tick(12);

			wi.LeadTime.Should().Be(4);
		}
		
		[TestMethod]
		public void SomeCompeletedWorkItemsMustGenerateCorrespondentCountOfCompletedWorkitems()
		{
			var wp = CreateWorkProcess();

			wp.Push(new WorkItem(1));
			wp.Push(new WorkItem(2));
			wp.Push(new WorkItem(3));

			wp.Tick(24);

			wp.CompletedWorkItems.Should().Be(3);
		}
				
		[TestMethod]
		public void WorkProcessShouldPushToInProgressQueueWithPushStrategy()
		{
			var op1 = new Operation();
			WorkProcess wp = new WorkProcess(new WorkProcessPushStrategy())
				.AddOperation(op1)
				.AddOperation(new Operation(2))
				.AddOperation(new Operation());

			wp.Push(new WorkItem());

			op1.InProgress.Count.Should().Be(1);
			wp.WorkInProgress.Should().Be(1);
		}


		[TestMethod]
		public void WorkProcessShouldPushToInputQueueWithPullStrategy()
		{
			var op1 = new Operation();
			WorkProcess wp = new WorkProcess(new WorkProcessPullStrategy())
				.AddOperation(op1)
				.AddOperation(new Operation(2))
				.AddOperation(new Operation());

			wp.Push(new WorkItem());

			op1.InProgress.Count.Should().Be(0);
			wp.InputQueue.Count.Should().Be(1);
			wp.WorkInProgress.Should().Be(0);
		}

		[TestMethod]
		public void ShouldMoveCompletedWorkItemsToOutputQueueWithPullStrategy()
		{
			var op1 = new Operation();  // push
			WorkProcess wp = new WorkProcess(new WorkProcessPullStrategy())
				.AddOperation(op1)
				.AddOperation(new Operation())
				.AddOperation(new Operation());

			wp.Push(new WorkItem(1));

			wp.Tick(9);

			wp.Done.Should().HaveCount(1);
		}

		[TestMethod]
		public void ShouldMoveCompletedWorkItemsToOutputQueueWithPushStrategy()
		{
			var op1 = new Operation();  // push
			WorkProcess wp = new WorkProcess(new WorkProcessPushStrategy())
				.AddOperation(op1)
				.AddOperation(new Operation())
				.AddOperation(new Operation());

			wp.Push(new WorkItem(1));

			wp.Tick(9);

			wp.Done.Should().HaveCount(1);
		}
	}
}

﻿using System;
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

		[TestMethod]
		public void WorkItemMustRichFirstOperationAfterFirstTick()
		{
			var wp = CreateWorkProcess();

			wp
				.Push(new WorkItem(1));

			wp.Tick();

			wp.Operations[0].WorkInProgress.Should().Be(1);
			wp.Operations[1].WorkInProgress.Should().Be(0);
		}

		[TestMethod]
		public void WorkItemMustRichSecondOperationAfterCompletedFirst()
		{
			var wp = CreateWorkProcess();

			wp
				.Push(new WorkItem(1));

			wp.Tick(4);

			wp.Operations[0].WorkInProgress.Should().Be(0);
			wp.Operations[1].WorkInProgress.Should().Be(1);
		}

		[TestMethod]
		public void WorkItemMustRichDone()
		{
			var wp = CreateWorkProcess();

			wp.Push(new WorkItem(1));

			wp.Tick(12);

			wp.Done.Count.Should().Be(1);
		}

		[TestMethod]
		public void AllPushedWorkItemsMustRichDone()
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
	}
}

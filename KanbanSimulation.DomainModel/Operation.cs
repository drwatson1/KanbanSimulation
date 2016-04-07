using System;
using System.Collections.Generic;

namespace KanbanSimulation.DomainModel
{
	public class Operation : IWorkItemQueue
	{
		public IWorkItemQueue InputQueue;
		public IWorkItemQueue OutputQueue;

		private WorkItem CurrentWorkItem; // -> Соответствует InProgress.Top, если над ним ведётся работа

		private readonly WorkItemQueue InProgressQueue = new WorkItemQueue();
		private readonly WorkItemQueue DoneQueue = new WorkItemQueue();

		public readonly int OperationComplexity;
		public IReadOnlyList<WorkItem> InProgress => InProgressQueue.Items;
		public IReadOnlyList<WorkItem> Done => DoneQueue.Items;

		public Operation(IWorkItemQueue pullFrom, IWorkItemQueue pushTo, int complexity = 1)
		{
			if (pullFrom == null || pushTo == null || complexity < 1)
				throw new ArgumentException();

			InputQueue = pullFrom;
			OutputQueue = pushTo;

			OperationComplexity = complexity;
		}

		public Operation(int complexity = 1)
		{
			if (complexity < 1)
				throw new ArgumentException();

			InputQueue = InProgressQueue;
			OutputQueue = DoneQueue;

			OperationComplexity = complexity;
		}

		// Берём в работу новый WorkItem
		public void TakeNewWorkItems()
		{
			if (!InputQueue.Empty)
			{
				InProgressQueue.Push(InputQueue.Pull());
			}

			// Готовимся приняться за работу над очередным WI
			if (CurrentWorkItem == null && !InProgressQueue.Empty)
			{
				CurrentWorkItem = InProgressQueue.Top;
				CurrentWorkItem.StartNewOperation();
			}
		}

		public void DoWork()
		{
			InProgressQueue.Tick();
			DoneQueue.Tick();
		}

		public void MoveCompletedWorkItems()
		{
			if (CurrentWorkItem == null)
				return;
			if (CurrentWorkItem.CurrentOperationProgress >= OperationComplexity)
			{
				OutputQueue.Push(InProgressQueue.Pull());
				CurrentWorkItem = null; // Закончили работу
			}
		}

		public int WorkInProgress => InProgressQueue.Count + DoneQueue.Count;

		#region IWorkItemQueue implementation

		public bool Empty => DoneQueue.Empty;

		public WorkItem Pull() => DoneQueue.Pull();

		public void Push(WorkItem wi) => InProgressQueue.Push(wi);

		#endregion IWorkItemQueue implementation
	}
}
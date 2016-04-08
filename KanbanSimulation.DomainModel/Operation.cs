using KanbanSimulation.Core;
using KanbanSimulation.Core.Interfaces;
using KanbanSimulation.DomainModel.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class Operation : EventSource, IWorkItemQueue
	{
		public IWorkItemQueue InputQueue;
		public IWorkItemQueue OutputQueue;

		private WorkItem CurrentWorkItem; // -> Соответствует InProgress.Top, если над ним ведётся работа

		internal readonly WorkItemQueue InProgressQueue = new WorkItemQueue();
		internal readonly WorkItemQueue DoneQueue = new WorkItemQueue();

		public readonly int OperationComplexity;
		public IReadOnlyList<WorkItem> InProgress => InProgressQueue.Items;
		public IReadOnlyList<WorkItem> Done => DoneQueue.Items;

		public Operation(IWorkItemQueue pullFrom, IWorkItemQueue pushTo, int complexity = 1, int id = 0)
			: base(id)
		{
			if (pullFrom == null || pushTo == null || complexity < 1)
				throw new ArgumentException();

			InputQueue = pullFrom;
			OutputQueue = pushTo;

			OperationComplexity = complexity;
		}

		public Operation(int complexity = 1, int id = 0)
			: base(id)
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
			if (!object.ReferenceEquals(InProgressQueue, InputQueue) && !InputQueue.Empty)
			{
				InProgressQueue.Push(InputQueue.Pull());
			}

			// Готовимся приняться за работу над очередным WI
			if (CurrentWorkItem == null && !InProgressQueue.Empty)
			{
				CurrentWorkItem = InProgressQueue.Top;
				CurrentWorkItem.StartNewOperation();
			}

			CollectEvents();
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

			CollectEvents();
		}

		public int WorkInProgress => InProgressQueue.Count + DoneQueue.Count;

		#region IWorkItemQueue implementation

		public bool Empty => DoneQueue.Empty;

		public WorkItem Pull() => DoneQueue.Pull();

		public void Push(WorkItem wi)
		{
			InProgressQueue.Push(wi);
			CollectEvents();
		}

		#endregion IWorkItemQueue implementation

		public override string ToString()
		{
			return $"InProgress: {InProgress.Count}, Done: {Done.Count}";
		}

		private void CollectEvents()
		{
			var newEvents = new List<IDomainEvent>();
			newEvents.AddRange(InProgressQueue.DomainEvents);
			newEvents.AddRange(DoneQueue.DomainEvents);

			// WIP changed if push and pull events quantity does not equals
			int pullCount = newEvents.Count(x =>
			{
				var e = x as WorkItemQueueChangedEvent;
				if (e == null)
					return false;
				return e.Operation == WorkItemQueueChangedEvent.QueueOperation.Pull;
			});
			int pushCount = newEvents.Count(x =>
			{
				var e = x as WorkItemQueueChangedEvent;
				if (e == null)
					return false;
				return e.Operation == WorkItemQueueChangedEvent.QueueOperation.Push;
			});

			newEvents.ForEach(AddDomainEvent);

			if (pullCount != pushCount)
				AddDomainEvent(new WorkInProgressChangedEvent(this));

			InProgressQueue.ClearEvents();
			DoneQueue.ClearEvents();
		}
	}
}
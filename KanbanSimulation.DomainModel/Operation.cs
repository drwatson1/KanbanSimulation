using KanbanSimulation.Core;
using KanbanSimulation.Core.Interfaces;
using KanbanSimulation.DomainModel.Events;
using KanbanSimulation.DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class Operation : EventSource, IInputQueue, IOutputQueue, IOperation
	{
		#region Private properties

		private static IWorkProcessStrategy DefaultStrategy = new WorkProcessPushStrategy();
		private WorkItem CurrentWorkItem; // -> Соответствует InProgress.Top, если над ним ведётся работа

		private readonly WorkItemQueue InProgressQueue = new WorkItemQueue();
		private readonly WorkItemQueue DoneQueue = new WorkItemQueue();

		#endregion Private properties

		#region Public properties

		public IWorkProcessStrategy Strategy { get; set; }
		public IInputQueue InputQueue { get; set; }
		public IOutputQueue OutputQueue { get; set; }

		public int Complexity { get; private set; }
		public IReadOnlyList<IWorkItem> InProgress => InProgressQueue;
		public IReadOnlyList<IWorkItem> Done => DoneQueue;
		public int WorkInProgress => InProgressQueue.Count + DoneQueue.Count;

		#endregion Public properties

		#region ctors

		public Operation(IWorkProcessStrategy strategy, IInputQueue pullFrom, IOutputQueue pushTo, int complexity = 1, int id = 0)
			: base(id)
		{
			if (pushTo == null)
				throw new ArgumentNullException(nameof(pushTo));
			if (pullFrom == null)
				throw new ArgumentNullException(nameof(pullFrom));
			if (strategy == null)
				throw new ArgumentNullException(nameof(strategy));
			if (complexity < 1)
				throw new ArgumentException(nameof(complexity));

			InputQueue = pullFrom;
			OutputQueue = pushTo;

			Complexity = complexity;
			Strategy = strategy;
		}

		public Operation(IInputQueue pullFrom, IOutputQueue pushTo, int complexity = 1, int id = 0)
			: this(DefaultStrategy, pullFrom, pushTo, complexity, id)
		{
		}

		public Operation(IWorkProcessStrategy strategy, int complexity = 1, int id = 0)
			: base(id)
		{
			if (strategy == null)
				throw new ArgumentNullException(nameof(strategy));

			if (complexity < 1)
				throw new ArgumentException();

			InputQueue = InProgressQueue;
			OutputQueue = DoneQueue;

			Complexity = complexity;
			Strategy = strategy;
		}

		public Operation(int complexity = 1, int id = 0)
			: this(DefaultStrategy, complexity, id)
		{
		}

		#endregion ctors

		#region WorkFlow public methods

		public void TakeNewWorkItems()
		{
			Strategy.Pull(InputQueue, InProgressQueue);

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

			if (CurrentWorkItem.CurrentOperationProgress >= Complexity)
			{
				Strategy.Push(InProgressQueue.Pull(), DoneQueue, OutputQueue);

				CurrentWorkItem = null; // Закончили работу
			}

			CollectEvents();
		}

		#endregion WorkFlow public methods

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
using KanbanSimulation.Core;
using KanbanSimulation.DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class Operation : Entity, IInputQueue, IOutputQueue, IOperation
	{
		#region Private properties

		private WorkItem CurrentWorkItem; // -> Соответствует InProgress.Top, если над ним ведётся работа

		public readonly WorkItemQueue InProgressQueue = new WorkItemQueue();
		public readonly WorkItemQueue DoneQueue = new WorkItemQueue();
		private int ActiveWorkItemsCount => CurrentWorkItem != null ? 1 : 0;
		private bool HaveCurrentWorkItem => ActiveWorkItemsCount > 0;

		public IConstraint Constraint { get; set; } = new DefaultConstraint();

		#endregion Private properties

		#region Public properties

		public IInputQueue InputQueue { get; set; }
		public IOutputQueue OutputQueue { get; set; }

		public int Complexity { get; private set; }
		public IReadOnlyList<IWorkItem> InProgress => HaveCurrentWorkItem ? InProgressQueue.Concat(new List<IWorkItem> { WorkedOn }).ToList() : InProgressQueue.ToList();

		public IWorkItem WorkedOn => CurrentWorkItem;

		public IReadOnlyList<IWorkItem> Done => DoneQueue;
		public int WorkInProgress => InProgress.Count + DoneQueue.Count;

		#endregion Public properties

		#region ctors

		public Operation(IInputQueue pullFrom, IOutputQueue pushTo, int complexity = 1, int id = 0)
			: this(complexity, id)
		{
			if (pushTo == null)
				throw new ArgumentNullException(nameof(pushTo));
			if (pullFrom == null)
				throw new ArgumentNullException(nameof(pullFrom));

			InputQueue = pullFrom;
			OutputQueue = pushTo;
		}

		public Operation(int complexity = 1, int id = 0)
			: base(id)
		{
			if (complexity < 1)
				throw new ArgumentException();

			InputQueue = InProgressQueue;
			OutputQueue = DoneQueue;

			Complexity = complexity;
		}

		#endregion ctors

		#region WorkFlow public methods

		public void TakeNewWorkItems()
		{
			if (!Constraint.CanTake(1))
				return;

			// InputQueue and InProgressQueue may be the same
			if (!InputQueue.Empty && !object.ReferenceEquals(InProgressQueue, InputQueue) )
			{
				InProgressQueue.Push(InputQueue.Pull());
			}

			if (CurrentWorkItem == null && !InProgressQueue.Empty)
			{
				CurrentWorkItem = InProgressQueue.Pull();
				CurrentWorkItem.StartNewOperation();
			}
		}

		public void DoWork()
		{
			CurrentWorkItem?.Tick();

			InProgressQueue.Tick();
			DoneQueue.Tick();
		}

		public void MoveCompletedWorkItems()
		{
			if (CurrentWorkItem == null)
				return;

			if (CurrentWorkItem.CurrentOperationProgress >= Complexity)
			{
				OutputQueue.Push(CurrentWorkItem);

				CurrentWorkItem = null; // Complete work
			}
		}

		#endregion WorkFlow public methods

		#region IWorkItemQueue implementation

		public bool Empty => DoneQueue.Empty;

		public WorkItem Pull() => DoneQueue.Pull();

		public void Push(WorkItem wi)
		{
			InProgressQueue.Push(wi);
		}

		#endregion IWorkItemQueue implementation

		public override string ToString()
		{
			return $"InProgress: {InProgress.Count}, Done: {Done.Count}";
		}
	}
}
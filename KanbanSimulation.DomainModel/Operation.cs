﻿using KanbanSimulation.Core;
using KanbanSimulation.Core.Interfaces;
using KanbanSimulation.DomainModel.Events;
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

		#endregion Private properties

		#region Public properties

		public IInputQueue InputQueue { get; set; }
		public IOutputQueue OutputQueue { get; set; }

		public int Complexity { get; private set; }
		public IReadOnlyList<IWorkItem> InProgress => InProgressQueue;
		public IReadOnlyList<IWorkItem> Done => DoneQueue;
		public int WorkInProgress => InProgressQueue.Count + DoneQueue.Count + ActiveWorkItemsCount;

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
			// Готовимся приняться за работу над очередным WI
			if (CurrentWorkItem == null && !InputQueue.Empty)
			{
				CurrentWorkItem = InputQueue.Pull();
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
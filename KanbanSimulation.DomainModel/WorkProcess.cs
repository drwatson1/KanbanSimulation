﻿using KanbanSimulation.Core;
using KanbanSimulation.DomainModel.Events;
using KanbanSimulation.DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcess : EventSource, IOperation
	{
		#region private properties

		private readonly List<Operation> operations = new List<Operation>();
		private readonly WorkItemQueue inputQueue;
		private readonly WorkItemQueue outputQueue;
		private Operation LastOperation;
		private StateMachine stateMachine;
		private IWorkProcessStrategy Strategy;

		#endregion private properties

		#region Public properties

		public IReadOnlyList<IWorkItem> InputQueue => inputQueue;
		public ICompletedWorkItems Done => outputQueue;
		public IReadOnlyList<IOperation> Operations => operations;
		public int ElapsedTicks { get; private set; }
		public int CompletedWorkItems { get; private set; }
		public readonly string Name;

		#endregion Public properties

		#region IOperation implementation

		public int Complexity
		{
			get
			{
				return Operations.Sum(op => op.Complexity);
			}
		}

		IReadOnlyList<IWorkItem> IOperation.InProgress
		{
			get
			{
				var inProgess = new List<IWorkItem>();
				operations.ForEach(op => inProgess.AddRange(op.InProgress));
				operations.ForEach(op => inProgess.AddRange(op.Done));

				return inProgess;
			}
		}

		IReadOnlyList<IWorkItem> IOperation.Done => outputQueue;

		public int WorkInProgress { get; private set; }

		#endregion IOperation implementation

		#region ctors

		public WorkProcess(IWorkProcessStrategy strategy, string name = "", int id = 0)
			: this(name, id)
		{
			Strategy = strategy;
		}

		public WorkProcess(string name = "", int id = 0)
			: this(new WorkItemQueue(1), new WorkItemQueue(2), name, id)
		{
		}

		protected WorkProcess(WorkItemQueue input, WorkItemQueue output, string name, int id = 0)
			: base(id)
		{
			inputQueue = input;
			outputQueue = output;
			Name = name;

			ConfigureStateMachine();
		}

		#endregion ctors

		#region Public methods

		public WorkProcess AddOperation(Operation operation)
		{
			operations.Add(operation);

			if (LastOperation != null)
			{
				LastOperation.OutputQueue = operation;
			}
			else
			{
				operation.InputQueue = inputQueue;
			}

			LastOperation = operation;
			LastOperation.OutputQueue = outputQueue;

			if (Strategy != null)
			{
				operation.Strategy = Strategy;
			}

			return this;
		}

		public WorkProcess Push(WorkItem workItem)
		{
			inputQueue.Push(workItem);

			return this;
		}

		public WorkProcess Tick(int count = 1)
		{
			if (count < 1)
				throw new ArgumentException("Count must be greater zero");

			for (int i = 0; i < count; ++i)
			{
				stateMachine.NextStep();
				CalculateWorkInProgress();
			}

			CollectEvents();

			ElapsedTicks += count;
			
			return this;
		}

		#endregion Public methods

		#region Private methods

		private void CollectEvents()
		{
			// If outputQueue was pushed with work item - this work item was completed. So we must create event about this
			var completed = outputQueue.DomainEvents.Where(e => (e is WorkItemQueueChangedEvent) && (e as WorkItemQueueChangedEvent).Operation == WorkItemQueueChangedEvent.QueueOperation.Push).Cast<WorkItemQueueChangedEvent>();

			operations.ForEach(CollectEvents);

			foreach (var e in completed)
			{
				AddDomainEvent(new WorkCompletedEvent(this, e.WorkItem));
				++CompletedWorkItems;
			}

			outputQueue.ClearEvents();
			inputQueue.ClearEvents();
		}

		private void CollectEvents(Operation op)
		{
			op.DomainEvents.ToList().ForEach(AddDomainEvent);
			op.ClearEvents();
		}

		private void CalculateWorkInProgress()
		{
			var oldWorkInProgress = WorkInProgress;
			WorkInProgress = operations.Sum(o => o.WorkInProgress);

			if (WorkInProgress != oldWorkInProgress)
			{
				AddDomainEvent(new WorkInProgressChangedEvent(this));
			}
		}

		private void ConfigureStateMachine()
		{
			var s3 = new State(() => { operations.ForEach(op => op.MoveCompletedWorkItems()); return true; });
			var s2 = new State(() => { operations.ForEach(op => op.DoWork()); return true; }, s3);
			var s1 = new State(() => { operations.ForEach(op => op.TakeNewWorkItems()); return true; }, s2);

			stateMachine = new StateMachine(s1);
		}

		#endregion Private methods
	}
}
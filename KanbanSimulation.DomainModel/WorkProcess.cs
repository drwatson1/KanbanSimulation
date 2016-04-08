﻿using KanbanSimulation.Core;
using KanbanSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcess: EventSource
	{
		private readonly List<Operation> operations = new List<Operation>();
		private readonly WorkItemQueue inputQueue = new WorkItemQueue();
		private readonly WorkItemQueue outputQueue = new WorkItemQueue();
		private Operation LastOperation;
		private StateMachine stateMachine;

		public IReadOnlyList<IWorkItem> InputQueue => inputQueue.Items.ToList();
		public IReadOnlyList<IWorkItem> Done => outputQueue.Items.ToList();

		public IReadOnlyList<IOperation> Operations => operations;

		public WorkProcess(int id = 0)
			:	base(id)
		{
			ConfigureStateMachine();
		}

		public WorkProcess AddOperation(Operation operation)
		{
			operations.Add(operation);

			if( LastOperation != null)
			{
				LastOperation.OutputQueue = operation;
			}
			else
			{
				operation.InputQueue = inputQueue;
			}

			LastOperation = operation;
			LastOperation.OutputQueue = outputQueue;

			return this;
		}

		public WorkProcess Push(WorkItem workItem)
		{
			inputQueue.Push(workItem);

			return this;
		}

		private void CollectEvents()
		{
			operations.ForEach(CollectEvents);
		}

		private void CollectEvents(Operation op)
		{
			op.DomainEvents.ToList().ForEach(AddDomainEvent);
			op.ClearEvents();
		}

		public WorkProcess Tick(int count = 1)
		{
			for (int i = 0; i < count; ++i)
			{
				stateMachine.NextStep();
			}

			CollectEvents();

			return this;
		}

		private void ConfigureStateMachine()
		{
			var s3 = new State(() => operations.ForEach(op => op.MoveCompletedWorkItems()), State.NullObject);
			var s2 = new State(() => operations.ForEach(op => op.DoWork()), s3);
			var s1 = new State(() => operations.ForEach(op => op.TakeNewWorkItems()), s2);

			stateMachine = new StateMachine(s1);
		}

		private class StateMachine
		{
			private readonly State FirstState;
			private State CurrentState;
			private readonly bool Cycle;

			public StateMachine(State firstState, bool cycle = true)
			{
				if(firstState == null )
				{
					throw new ArgumentException(nameof(firstState));
				}

				FirstState = firstState;
				CurrentState = FirstState;
				Cycle = cycle;
			}

			public void NextStep()
			{
				CurrentState.StateAction();
				
				if(CurrentState.NextState.IsNull && Cycle)
				{
					CurrentState = FirstState;
				}
				else
				{
					CurrentState = CurrentState.NextState;
				}
			}
		}

		private class State
		{
			public readonly State NextState;
			public readonly Action StateAction;

			public bool IsNull => NextState == this;

			static public readonly State NullObject = new State();

			private State()	//	NullObject
			{
				NextState = this;
				StateAction = new Action(() => { } );
			}

			public State(Action action, State next)
			{
				NextState = next;
				StateAction = action;
			}
		}
	}
}
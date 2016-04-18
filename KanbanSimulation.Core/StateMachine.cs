using System;

namespace KanbanSimulation.Core
{
	public class StateMachine
	{
		private readonly State FirstState;
		private State CurrentState;
		private readonly bool Cycle;

		public int CurrentStateId => CurrentState.Id;

		public StateMachine(State firstState, bool cycle = true)
		{
			if (firstState == null)
			{
				throw new ArgumentException(nameof(firstState));
			}

			FirstState = firstState;
			CurrentState = FirstState;
			Cycle = cycle;
		}

		public bool NextStep()
		{
			if (!CurrentState.StateAction())
				return !IsFinished;

			if (CurrentState.NextState.IsNull && Cycle)
			{
				CurrentState = FirstState;
			}
			else
			{
				CurrentState = CurrentState.NextState;
			}

			return !IsFinished;
		}

		public bool IsFinished => CurrentState.IsNull;
	}
}
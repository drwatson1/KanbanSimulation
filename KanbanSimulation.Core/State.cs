using System;

namespace KanbanSimulation.Core
{
	public class State
	{
		public readonly State NextState;
		public readonly Func<bool> StateAction;

		public bool IsNull => NextState == this;

		static public readonly State NullObject = new State();

		private State() //	NullObject
		{
			NextState = this;
			StateAction = new Func<bool>(() => false);
		}

		public State(Func<bool> action, State next)
		{
			NextState = next;
			StateAction = action;
		}

		public State(Func<bool> action)
			: this(action, NullObject)
		{
		}
	}
}
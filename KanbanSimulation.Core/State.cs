using System;

namespace KanbanSimulation.Core
{
	public class State
	{
		public readonly State NextState;
		public readonly Func<bool> StateAction;
		public readonly int Id;

		public bool IsNull => NextState == this;

		static public readonly State NullObject = new State();

		private State() //	NullObject
		{
			NextState = this;
			StateAction = new Func<bool>(() => false);
		}

		public State(Func<bool> action, State next, int id = 0)
		{
			NextState = next;
			StateAction = action;
			Id = id;
		}

		public State(Func<bool> action, int id = 0)
			: this(action, NullObject, id)
		{
		}
	}
}
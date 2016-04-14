using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KanbanSimulation.Core.Tests
{
	[TestClass]
	public class StateMachineTest
	{
		[TestMethod]
		public void MustMoveToNextStateIfActionReturnsTrue()
		{
			bool s2Call = false;
			var s2 = new State(() => { s2Call = true; return true; } );
			var s1 = new State(() => true, s2);

			var sm = new StateMachine(s1);

			sm.NextStep();
			sm.NextStep();

			s2Call.Should().BeTrue();
		}

		[TestMethod]
		public void MustNotMoveToNextStateIfActionReturnsFalse()
		{
			bool s2Call = false;
			var s2 = new State(() => { s2Call = true; return true; });
			var s1 = new State(() => false, s2);

			var sm = new StateMachine(s1);

			sm.NextStep();
			sm.NextStep();

			s2Call.Should().BeFalse();
		}

		[TestMethod]
		public void IsFinishedMustBeFalseAfterInitiating()
		{
			var s2 = new State(() => true);
			var s1 = new State(() => true, s2);

			var sm = new StateMachine(s1);

			sm.IsFinished.Should().BeFalse();
		}

		[TestMethod]
		public void IsFinishedMustBeFalseAfterFirstStep()
		{
			var s2 = new State(() => true);
			var s1 = new State(() => true, s2);

			var sm = new StateMachine(s1);

			sm.NextStep();

			sm.IsFinished.Should().BeFalse();
		}

		[TestMethod]
		public void IsFinishedMustBeTrueAfterLastStateIfNoCycle()
		{
			var s2 = new State(() => true );
			var s1 = new State(() => true, s2);

			var sm = new StateMachine(s1, false);

			sm.NextStep();
			sm.NextStep();

			sm.IsFinished.Should().BeTrue();
		}


		[TestMethod]
		public void IsFinishedMustBeFalseAfterLastStateIfCycle()
		{
			var s2 = new State(() => true);
			var s1 = new State(() => true, s2);

			var sm = new StateMachine(s1, true);

			sm.NextStep();
			sm.NextStep();

			sm.IsFinished.Should().BeFalse();
		}

		[TestMethod]
		public void FirstStateMustBeCalledAfterCycle()
		{
			int s1CallsCount = 0;
			var s2 = new State(() => true);
			var s1 = new State(() => { ++s1CallsCount; return true; }, s2);

			var sm = new StateMachine(s1, true);

			sm.NextStep();
			sm.NextStep();
			sm.NextStep();

			s1CallsCount.Should().Be(2);
		}
	}
}

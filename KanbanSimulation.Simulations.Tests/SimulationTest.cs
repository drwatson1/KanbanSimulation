using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KanbanSimulation.Simulations;
using KanbanSimulation.DomainModel;
using FluentAssertions;

namespace KanbanSimulation.Simulations.Tests
{
	[TestClass]
	public class SimulationTest
	{
		private static Simulation GetSimulation()
		{
			var wp = new WorkProcess()
				.AddOperation(new Operation())
				.AddOperation(new Operation(3))
				.AddOperation(new Operation());

			return new Simulation(wp);
		}

		[TestMethod]
		public void WipInFirstOperationAlwaysShouldBeOne()
		{
			var s = GetSimulation();

			s.Tick();
			s.Process.Operations[0].WorkInProgress.Should().Be(1);

			s.Tick();
			s.Process.Operations[0].WorkInProgress.Should().Be(1);

			s.Tick();
			s.Process.Operations[0].WorkInProgress.Should().Be(1);
		}

		[TestMethod]
		public void CompletedWorkItemsShouldBeCorrectlyCalculated()
		{
			var s = GetSimulation();

			s.Tick();

			s.Tick();
			s.Tick();
			s.Tick();
			s.Tick(); // -> First WI moved to 3 op

			s.Process.CompletedWorkItems.Should().Be(0);

			s.Tick(); // -> First WI moved to done; 3 op must be empty, Second wi = 1
			s.Process.CompletedWorkItems.Should().Be(1);
			s.Process.Operations[2].WorkInProgress.Should().Be(0);

			s.Tick(); // Second wi = 2
			s.Tick(); // Second wi = 3, moved to op 3
			s.Process.Operations[2].WorkInProgress.Should().Be(1);
			s.Process.CompletedWorkItems.Should().Be(1);

			s.Tick();
			s.Process.CompletedWorkItems.Should().Be(2);
			s.Process.Operations[2].WorkInProgress.Should().Be(0);
		}

		[TestMethod]
		public void AfterFirstCompletedWorkItemSimulationShouldMoveToNextState()
		{
			var s = GetSimulation();

			while(s.Process.CompletedWorkItems == 0)
			{
				s.Tick();
			}

			s.CurrentWorkFlowState.Should().Be(2);

			while(s.CurrentWorkFlowState == 2)
			{
				s.Tick();
			}

			s.CurrentWorkFlowState.Should().Be(3);
		
		}
	}
}

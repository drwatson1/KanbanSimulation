using KanbanSimulation.Core;
using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System.Linq;

namespace KanbanSimulation.Simulations
{
	public class Simulation
	{
		private readonly IdGeneratorService identity = new IdGeneratorService();

		#region StateMachine and it's state variables

		private int CurrentTicks;
		private WorkItem EthalonWorkItem;
		private StateMachine Workflow;
		private int FirstCycleLeadTime;
		private int FirstCycleElapsedTicks;

		#endregion StateMachine and it's state variables

		#region Public properties

		public readonly WorkProcess Process;
		public decimal Throughput;
		public int WorkInProgress;
		public int LeadTime;

		#endregion Public properties

		public Simulation(WorkProcess process)
		{
			Process = process;

			ConfigureStateMachine();
		}

		public bool Tick()
		{
			return Workflow.NextStep();
		}

		#region Private methods

		private void ConfigureStateMachine()
		{
			var s3 = new State(() =>
			{
				if (OutputQueueContainsWorkItem(EthalonWorkItem))
					return true;

				if (EthalonWorkItem == null)
				{
					EthalonWorkItem = new WorkItem(identity.NextId());
					Process.Push(EthalonWorkItem);
				}

				Process.Done.Clear();
				Process.Tick();

				if (OutputQueueContainsWorkItem(EthalonWorkItem))
					return false;

				LeadTime = EthalonWorkItem.LeadTime;

				return true;
			});

			var s2 = new State(() =>
			{
				if (CurrentTicks >= FirstCycleElapsedTicks * 10)
					return true;

				Process.Tick();
				++CurrentTicks;

				if (CurrentTicks < FirstCycleElapsedTicks * 10)
					return false;

				Throughput = Process.Done.Count / 10;
				WorkInProgress = Process.WorkInProgress;

				Process.Done.Clear();

				return true;
			}, s3);

			var s1 = new State(() =>
			{
				if (!Process.Done.Empty)
					return true;

				Tick();

				if (Process.Done.Empty)
					return false;

				FirstCycleLeadTime = Process.Done[0].LeadTime;
				FirstCycleElapsedTicks = Process.ElapsedTicks;

				Process.Done.Clear();

				return true;
			}, s2);

			Workflow = new StateMachine(s1, false);
		}

		private bool OutputQueueContainsWorkItem(WorkItem wi)
		{
			return Process.Done.Count(i => i.Equals(wi)) > 0;
		}

		#endregion Private methods
	}
}
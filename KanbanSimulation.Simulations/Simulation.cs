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

		private int CurrentTicks => Process.ElapsedTicks - FirstCycleElapsedTicks;
		private WorkItem EthalonWorkItem;
		private StateMachine Workflow;
		public int FirstCycleLeadTime { get; private set; }
		private int FirstCycleElapsedTicks;

		#endregion StateMachine and it's state variables

		#region Public properties

		public readonly WorkProcess Process;
		public decimal Throughput;
		public int WorkInProgress;
		public int LeadTime;
		public int CurrentState => Workflow.CurrentStateId;

		#endregion Public properties

		public Simulation(WorkProcess process)
		{
			Process = process;

			ConfigureStateMachine();
		}

		public bool Tick()
		{
			Process.ClearEvents();
			Workflow.NextStep();
			return Workflow.IsFinished;
		}

		public bool IsFinished => Workflow.IsFinished;

		#region Private methods

		private void ConfigureStateMachine()
		{
			var s3 = new State(() =>
			{
				if (EthalonWorkItem != null && OutputQueueContainsWorkItem(EthalonWorkItem))
					return true;

				if (EthalonWorkItem == null)
				{
					EthalonWorkItem = new WorkItem(identity.NextId());
					Process.Push(EthalonWorkItem);
				}

				Process.Done.Clear();
				Process.Tick(3);

				if (!OutputQueueContainsWorkItem(EthalonWorkItem))
					return false;

				LeadTime = EthalonWorkItem.LeadTime;

				return true;
			}, 3);

			var s2 = new State(() =>
			{
				if (CurrentTicks >= FirstCycleElapsedTicks * 10)
					return true;

				if (Process.InputQueue.Count == 0)
					Process.Push(new WorkItem(identity.NextId()));

				Process.Tick(3);

				if (CurrentTicks < FirstCycleElapsedTicks * 10)
					return false;

				Throughput = Process.Done.Count / ((decimal)CurrentTicks / FirstCycleElapsedTicks);
				WorkInProgress = Process.WorkInProgress;

				Process.Done.Clear();

				return true;
			}, s3, 2);

			var s1 = new State(() =>
			{
				if (!Process.Done.Empty)
					return true;

				if (Process.InputQueue.Count == 0)
					Process.Push(new WorkItem(identity.NextId()));

				Process.Tick(3);

				if (Process.Done.Empty)
					return false;

				FirstCycleLeadTime = Process.Done[0].LeadTime;
				FirstCycleElapsedTicks = Process.ElapsedTicks;

				Process.Done.Clear();

				return true;
			}, s2, 1);

			var s0 = new State(() =>
			{
				if (Process.InputQueue.Count == 0)
					Process.Push(new WorkItem(identity.NextId()));

				Process.Tick();

				return true;
			}, s1, 0);

			Workflow = new StateMachine(s0, false);
		}

		private bool OutputQueueContainsWorkItem(WorkItem wi)
		{
			if (wi == null)
				return false;

			return Process.Done.Count(i => i.Equals(wi)) > 0;
		}

		#endregion Private methods
	}
}
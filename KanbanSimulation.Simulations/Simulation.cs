using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System.Linq;

namespace KanbanSimulation.Simulations
{
	public class Simulation
	{
		private readonly IdGeneratorService identity = new IdGeneratorService();

		private int FirstCycleLeadTime;
		private int FirstCycleElapsedTicks;
		private decimal Throughput;
		private int WorkInProgress;
		private int FinalLeadTime;

		public readonly WorkProcess Process;

		public Simulation(WorkProcess process)
		{
			Process = process;
		}

		public void Tick()
		{
			Process.Push(new WorkItem(identity.NextId()));
			Process.Tick();
		}

		private bool Step1()
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
		}

		private int CurrentTicks;

		private bool Step2()
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
		}

		private WorkItem EthalonWorkItem;

		private bool Step3()
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

			FinalLeadTime = EthalonWorkItem.LeadTime;

			return true;
		}

		private bool OutputQueueContainsWorkItem(WorkItem wi)
		{
			return Process.Done.Count(i => i.Equals(wi)) > 0;
		}
	}
}
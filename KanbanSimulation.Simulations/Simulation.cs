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

		private void Step1()
		{
			while (Process.Done.Empty)
			{
				Tick();
			}

			FirstCycleLeadTime = Process.Done[0].LeadTime;
			FirstCycleElapsedTicks = Process.ElapsedTicks;

			Process.Done.Clear();
		}

		private void Step2()
		{
			Process.Tick(FirstCycleElapsedTicks * 10);

			Throughput = Process.Done.Count / 10;
			WorkInProgress = Process.WorkInProgress;

			Process.Done.Clear();
		}

		private void Step3()
		{
			var wiX = new WorkItem(identity.NextId());
			Process.Push(wiX);

			while (!OutputQueueContainsWorkItem(wiX.Id))
			{
				Process.Done.Clear();
				Process.Tick();
			}

			FinalLeadTime = wiX.LeadTime;
		}

		private bool OutputQueueContainsWorkItem(int id)
		{
			return Process.Done.Count(i => i.Id == id) > 0;
		}
	}
}
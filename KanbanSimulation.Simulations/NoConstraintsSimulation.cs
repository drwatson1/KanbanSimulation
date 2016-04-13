using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System.Linq;

namespace KanbanSimulation.Simulations
{
	public class NoConstraintsSimulation
	{
		private readonly WorkProcess process;
		private readonly IdGeneratorService identity = new IdGeneratorService();
		private readonly WorkItemQueue InputQueue = new WorkItemQueue();
		private readonly WorkItemQueue OutputQueue = new WorkItemQueue();

		private int FirstCycleLeadTime;
		private int FirstCycleElapsedTicks;
		private decimal Throughput;
		private int WorkInProgress;
		private int FinalLeadTime;

		public WorkProcess Process => process;

		public NoConstraintsSimulation()
		{
			process = new WorkProcess(InputQueue, OutputQueue)
				.AddOperation(new Operation())
				.AddOperation(new Operation())
				.AddOperation(new Operation())
				.AddOperation(new Operation())
				.AddOperation(new Operation())
				.AddOperation(new Operation())
				.AddOperation(new Operation(5))
				.AddOperation(new Operation())
				.AddOperation(new Operation());
		}

		public void Tick()
		{
			InputQueue.Push(new WorkItem(identity.NextId()));
			process.Tick();
		}

		private void Step1()
		{
			while (OutputQueue.Empty)
			{
				Tick();
			}

			FirstCycleLeadTime = OutputQueue.Top.LeadTime;
			FirstCycleElapsedTicks = process.ElapsedTicks;

			OutputQueue.Clear();
		}

		private void Step2()
		{
			process.Tick(FirstCycleElapsedTicks * 10);

			Throughput = OutputQueue.Count / 10;
			WorkInProgress = process.WorkInProgress;

			OutputQueue.Clear();
		}

		private void Step3()
		{
			var wiX = new WorkItem(identity.NextId());
			InputQueue.Push(wiX);

			while (!OutputQueueContainsWorkItem(wiX.Id))
			{
				OutputQueue.Clear();
				process.Tick();
			}

			FinalLeadTime = wiX.LeadTime;
		}

		private bool OutputQueueContainsWorkItem(int id)
		{
			return OutputQueue.Items.Count(i => i.Id == id) > 0;
		}
	}
}
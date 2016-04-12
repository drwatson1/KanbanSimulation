using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Simulations
{
    public class NoConstraintsSimulation
    {
		private readonly WorkProcess process;
		private readonly IdGeneratorService identity = new IdGeneratorService();

		public WorkProcess Process => process;

		public NoConstraintsSimulation()
		{
			process = new WorkProcess()
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
			process.Push(new WorkItem(identity.NextId()));
		}
	}
}

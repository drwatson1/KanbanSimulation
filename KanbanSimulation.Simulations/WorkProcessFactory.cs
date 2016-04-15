using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Simulations
{
	static public class WorkProcessFactory
	{
		private static readonly IdGeneratorService identity = new IdGeneratorService();

		static public WorkProcess CreateNoConstraintsWorkProcess()
		{
			return new WorkProcess()
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(5, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()));
		}
	}
}

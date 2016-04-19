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

		static public WorkProcess CreateNoConstraintsWorkProcess(int bottleneck = 5)
		{
			if (bottleneck < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(bottleneck));

			return new WorkProcess($"Work process with out constraints (bottleneck={bottleneck})")
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(bottleneck, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()));
		}
	}
}

using KanbanSimulation.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Simulations
{
	static public class WorkProcessFactory
	{
		static public WorkProcess CreateNoConstraintsWorkProcess()
		{
			return new WorkProcess()
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
	}
}

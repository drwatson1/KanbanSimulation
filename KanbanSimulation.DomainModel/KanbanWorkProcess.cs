using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel
{
	public class KanbanWorkProcess
		:	WorkProcess
	{
		private readonly uint Limit;

		public KanbanWorkProcess(uint limit, string name = "", int id = 0)
			:	base(new WorkProcessPullStrategy(), name, id)
		{
			Limit = limit;
		}

		protected override void OnAfterAddOperation(Operation operation)
		{
			operation.Constraint = new WipLimitConstraint(operation, Limit);
		}
	}
}

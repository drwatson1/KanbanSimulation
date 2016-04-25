using KanbanSimulation.DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel
{
	public class WipConstraint : IConstraint
	{
		private readonly IOperation Operation;
		private readonly uint Limit;

		public WipConstraint(IOperation operation, uint limit)
		{
			if (operation == null)
				throw new ArgumentNullException(nameof(operation));

			Operation = operation;
			Limit = limit;
		}

		public bool CanTake(uint count)
		{
			return Operation.WorkInProgress + count <= Limit;
		}
	}
}

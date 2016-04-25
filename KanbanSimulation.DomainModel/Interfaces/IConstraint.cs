using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IConstraint
	{
		bool CanTake(uint count);
	}
}

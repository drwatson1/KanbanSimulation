using KanbanSimulation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel.Events
{
	public class WorkProcessWorkInProgressChangedEvent: DomainEvent<WorkProcess>
	{
		public WorkProcessWorkInProgressChangedEvent(WorkProcess workProcess)
			:	base(workProcess)
		{

		}
	}
}

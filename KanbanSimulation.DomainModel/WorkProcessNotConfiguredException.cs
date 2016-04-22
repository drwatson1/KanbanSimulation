using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcessNotConfiguredException: Exception
	{
		public WorkProcessNotConfiguredException()
			:	base("Work process does not configured")
		{ }
	}
}

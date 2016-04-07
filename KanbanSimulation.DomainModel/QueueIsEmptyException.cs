using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel
{
	public class QueueIsEmptyException
		:	Exception
	{
		public QueueIsEmptyException()
			:	base("Queue is empty")
		{ }
	}
}

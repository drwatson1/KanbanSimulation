using KanbanSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IOutputQueue: IEntity
	{
		void Push(WorkItem wi);
	}
}

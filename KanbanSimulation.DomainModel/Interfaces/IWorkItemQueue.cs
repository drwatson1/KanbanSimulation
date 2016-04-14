using KanbanSimulation.Core.Interfaces;
using System.Collections.Generic;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IWorkItemQueue: IEntity, IReadOnlyList<IWorkItem>
	{
	}
}
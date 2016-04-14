using KanbanSimulation.Core.Interfaces;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IInputQueue : IEntity
	{
		bool Empty { get; }

		WorkItem Pull();
	}
}
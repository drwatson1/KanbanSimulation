using KanbanSimulation.Core.Interfaces;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IWorkItem : IEntity
	{
		int LeadTime { get; }
		int CurrentOperationProgress { get; }
	}
}
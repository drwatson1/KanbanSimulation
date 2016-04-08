using System.Collections.Generic;

namespace KanbanSimulation.DomainModel
{
	public interface IOperation
	{
		int Id { get; }
		IReadOnlyList<IWorkItem> Done { get; }
		IReadOnlyList<IWorkItem> InProgress { get; }
		int WorkInProgress { get; }
	}
}
using KanbanSimulation.Core.Interfaces;
using System.Collections.Generic;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IOperation : IEntity
	{
		int Complexity { get; }
		IReadOnlyList<IWorkItem> InProgress { get; }
		IWorkItem WorkedOn { get; }
		bool HaveWorkedOnItem { get; }
		IReadOnlyList<IWorkItem> Done { get; }
		int WorkInProgress { get; }
	}
}
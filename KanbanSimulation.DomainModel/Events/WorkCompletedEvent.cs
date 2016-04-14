using KanbanSimulation.Core;
using KanbanSimulation.DomainModel.Interfaces;

namespace KanbanSimulation.DomainModel.Events
{
	public class WorkCompletedEvent : DomainEvent<WorkProcess>
	{
		public readonly IWorkItem WorkItem;

		public WorkCompletedEvent(WorkProcess workProcess, IWorkItem wi)
			: base(workProcess)
		{
			WorkItem = wi;
		}
	}
}
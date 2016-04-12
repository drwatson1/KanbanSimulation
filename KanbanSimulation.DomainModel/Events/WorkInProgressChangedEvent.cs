using KanbanSimulation.Core;

namespace KanbanSimulation.DomainModel.Events
{
	public class WorkInProgressChangedEvent : DomainEvent<IOperation>
	{
		public WorkInProgressChangedEvent(IOperation operation)
			: base(operation)
		{
		}
	}
}
using KanbanSimulation.Core;
using KanbanSimulation.DomainModel.Interfaces;

namespace KanbanSimulation.DomainModel.Events
{
	public class WorkItemQueueChangedEvent : DomainEvent<IWorkItemQueue>
	{
		public enum QueueOperation
		{
			Pull, Push
		}

		public IWorkItem WorkItem { get; private set; }
		public QueueOperation Operation { get; private set; }

		public WorkItemQueueChangedEvent(IWorkItemQueue sender, IWorkItem workItem, QueueOperation operation)
			: base(sender)
		{
			WorkItem = workItem;
			Operation = operation;
		}
	}
}
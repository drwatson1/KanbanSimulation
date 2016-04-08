using KanbanSimulation.Core;

namespace KanbanSimulation.DomainModel.Events
{
	public class WorkItemQueueChangedEvent : DomainEvent<WorkItemQueue>
	{
		public enum QueueOperation
		{
			Pull, Push
		}

		public WorkItem WorkItem { get; private set; }
		public QueueOperation Operation { get; private set; }

		public WorkItemQueueChangedEvent(WorkItemQueue sender, WorkItem workItem, QueueOperation operation)
			: base(sender)
		{
			WorkItem = workItem;
			Operation = operation;
		}
	}
}
using KanbanSimulation.Core;
using System.Collections.Generic;
using System.Linq;
using System;
using KanbanSimulation.DomainModel.Events;

namespace KanbanSimulation.DomainModel
{
	public class WorkItemQueue : EventSource, IWorkItemQueue
	{
		private readonly Queue<WorkItem> queue = new Queue<WorkItem>();

		public IReadOnlyList<WorkItem> Items => queue.ToList();

		public void Push(WorkItem wi)
		{
			queue.Enqueue(wi);

			Raise(wi, WorkItemQueueChangedEvent.QueueOperation.Push);
		}

		private void Raise(WorkItem wi, WorkItemQueueChangedEvent.QueueOperation operation)
		{
			AddDomainEvent(new WorkItemQueueChangedEvent(this, wi, operation));
		}

		public WorkItem Pull()
		{
			if (Empty)
				throw new QueueIsEmptyException();


			var wi = queue.Dequeue();

			Raise(wi, WorkItemQueueChangedEvent.QueueOperation.Pull);

			return wi;
		}

		public WorkItem Top
		{
			get
			{
				if (Empty)
					throw new QueueIsEmptyException();

				return queue.Peek();
			}
		}

		public void Tick()
		{
			foreach (var wi in queue)
			{
				wi.Tick();
			}
		}

		public bool Empty => queue.Count == 0;

		public int Count => queue.Count;
	}
}
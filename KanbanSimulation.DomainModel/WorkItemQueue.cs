using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class WorkItemQueue : IWorkItemQueue
	{
		private Queue<WorkItem> queue = new Queue<WorkItem>();

		public WorkItemQueue()
		{ }

		public IReadOnlyList<WorkItem> Items => queue.ToList();

		public void Push(WorkItem wi)
		{
			queue.Enqueue(wi);
		}

		public WorkItem Pull()
		{
			if (Empty)
				throw new QueueIsEmptyException();

			return queue.Dequeue();
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
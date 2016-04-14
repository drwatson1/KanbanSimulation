using KanbanSimulation.Core;
using KanbanSimulation.DomainModel.Events;
using KanbanSimulation.DomainModel.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class WorkItemQueue : EventSource, IWorkItemQueue, IReadOnlyList<IWorkItem>, ICompletedWorkItems
	{
		#region Own interface

		public WorkItemQueue(int id = 0)
			: base(id)
		{
		}

		public WorkItem Top
		{
			get
			{
				if (Empty)
					throw new QueueIsEmptyException();

				return queue[0];
			}
		}

		public void Tick()
		{
			queue.ForEach(wi => wi.Tick());
		}

		#endregion Own interface

		#region IWorkItemQueue implementation

		public void Push(WorkItem wi)
		{
			queue.Add(wi);

			Raise(wi, WorkItemQueueChangedEvent.QueueOperation.Push);
		}

		public WorkItem Pull()
		{
			if (Empty)
				throw new QueueIsEmptyException();

			var wi = queue[0];
			queue.RemoveAt(0);

			Raise(wi, WorkItemQueueChangedEvent.QueueOperation.Pull);

			return wi;
		}

		public bool Empty => queue.Count == 0;

		#endregion IWorkItemQueue implementation

		#region IReadOnlyList<IWorkItem> implementation

		public IEnumerator<IWorkItem> GetEnumerator()
		{
			return queue.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return queue.GetEnumerator();
		}

		public int Count => queue.Count;

		public IWorkItem this[int index]
		{
			get
			{
				return queue[index];
			}
		}

		#endregion IReadOnlyList<IWorkItem> implementation

		#region ICompletedWorkItems implementation

		bool ICompletedWorkItems.Contains(IWorkItem wi)
		{
			return queue.SingleOrDefault(x => x.Equals(wi)) != null;
		}

		void ICompletedWorkItems.Remove(IWorkItem wi)
		{
			queue.RemoveAll(x => x.Equals(wi));
		}

		void ICompletedWorkItems.RemoveAt(int index)
		{
			queue.RemoveAt(index);
		}

		void ICompletedWorkItems.Clear()
		{
			queue.Clear();
		}

		#endregion ICompletedWorkItems implementation

		#region private members

		private readonly List<WorkItem> queue = new List<WorkItem>();

		private void Raise(WorkItem wi, WorkItemQueueChangedEvent.QueueOperation operation)
		{
			AddDomainEvent(new WorkItemQueueChangedEvent(this, wi, operation));
		}

		#endregion private members
	}
}
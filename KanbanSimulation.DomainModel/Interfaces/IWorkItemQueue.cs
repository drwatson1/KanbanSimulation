using System.Collections.Generic;

namespace KanbanSimulation.DomainModel
{
	public interface IWorkItemQueue
	{
		int Id { get; }
		bool Empty { get; }

		WorkItem Pull();

		void Push(WorkItem wi);
	}

	public interface ICompletedWorkItems: IReadOnlyList<IWorkItem>
	{
		bool Empty { get; }

		bool Contains(IWorkItem wi);

		void Remove(IWorkItem wi);

		void RemoveAt(int index);

		void Clear();
	}
}
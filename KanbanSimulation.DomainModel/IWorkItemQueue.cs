namespace KanbanSimulation.DomainModel
{
	public interface IWorkItemQueue
	{
		bool Empty { get; }

		WorkItem Pull();

		void Push(WorkItem wi);
	}
}
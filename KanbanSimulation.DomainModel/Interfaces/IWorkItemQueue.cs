namespace KanbanSimulation.DomainModel
{
	public interface IWorkItemQueue
	{
		int Id { get; }
		bool Empty { get; }

		WorkItem Pull();

		void Push(WorkItem wi);
	}
}
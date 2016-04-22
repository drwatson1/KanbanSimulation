namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IWorkProcessStrategy
	{
		void ConfigureInputQueue(Operation op, IInputQueue inputQueue);
		void ConfigureOutputQueue(Operation op, IOutputQueue outputQueue);
		void Push(Operation op, WorkItemQueue inputQueue, WorkItem wi);
	}
}
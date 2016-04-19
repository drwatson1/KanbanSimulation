namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface IWorkProcessStrategy
	{
		void Pull(IInputQueue inputQueue, IOutputQueue inProgressQueue);

		void Push(WorkItem wi, IOutputQueue doneQueue, IOutputQueue outputQueue);
	}
}
namespace KanbanSimulation.Core.Interfaces
{
	public interface IHandle<T> where T : IDomainEvent
	{
		void Handle(T args);
	}
}
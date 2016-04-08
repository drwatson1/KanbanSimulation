namespace KanbanSimulation.DomainModel
{
	public interface IWorkItem
	{
		int Id { get; }
		int LeadTime { get; }
	}
}
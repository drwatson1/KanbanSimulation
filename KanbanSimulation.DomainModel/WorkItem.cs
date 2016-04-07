using KanbanSimulation.Core;

namespace KanbanSimulation.DomainModel
{
	public class WorkItem : Entity<int>
	{
		public int LeadTime { get; private set; }
		public int CurrentOperationProgress { get; private set; }

		public WorkItem(int id)
			: base(id)
		{ }

		public void Tick()
		{
			++LeadTime;
			++CurrentOperationProgress;
		}

		// Начало новой операции
		public void StartNewOperation()
		{
			CurrentOperationProgress = 0;
		}
	}
}
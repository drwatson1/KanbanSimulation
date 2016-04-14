using KanbanSimulation.Core;
using KanbanSimulation.DomainModel.Interfaces;

namespace KanbanSimulation.DomainModel
{
	public class WorkItem : Entity, IWorkItem
	{
		public int LeadTime { get; private set; }
		public int CurrentOperationProgress { get; private set; }

		public WorkItem(int id = 0)
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
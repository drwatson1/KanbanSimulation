using KanbanSimulation.DomainModel.Interfaces;

namespace KanbanSimulation.DomainModel
{
	public class DefaultConstraint : IConstraint
	{
		public bool CanTake(uint count)
		{
			return true;
		}
	}
}
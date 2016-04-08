using KanbanSimulation.Core.Interfaces;

namespace KanbanSimulation.DomainModel.Services
{
	public class IdGeneratorService : IIdGeneratorService
	{
		private int StartNumber;

		public IdGeneratorService(int startNumber = 0)
		{
			StartNumber = startNumber;
		}

		public int NextId()
		{
			return ++StartNumber;
		}
	}
}
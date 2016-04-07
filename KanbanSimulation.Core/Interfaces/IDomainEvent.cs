using System;

namespace KanbanSimulation.Core.Interfaces
{
	public interface IDomainEvent
	{
		DateTime DateTimeEventOccurred { get; }
	}
}
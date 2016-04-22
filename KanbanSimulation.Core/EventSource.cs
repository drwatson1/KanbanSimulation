using KanbanSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Core
{
	public abstract class EventSource1 : Entity
	{
		private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();
		public virtual IReadOnlyList<IDomainEvent> DomainEvents => domainEvents;

		protected EventSource1(int id)
			:	base(id)
		{

		}

		protected virtual void AddDomainEvent(IDomainEvent newEvent)
		{
			domainEvents.Add(newEvent);
		}

		public virtual void ClearEvents()
		{
			domainEvents.Clear();
		}
	}
}

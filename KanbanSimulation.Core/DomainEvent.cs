using KanbanSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Core
{
	public class DomainEvent<TSource>: IDomainEvent
	{
		public DateTime DateTimeEventOccurred { get; private set; }
		public TSource Sender { get; private set; }

		public DomainEvent(TSource sender)
		{
			DateTimeEventOccurred = DateTime.Now;
			Sender = sender;
		}
	}
}

using KanbanSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Core
{
	public class DomainEvent<TSource>: IDomainEvent
		where TSource: class
	{
		public DateTime DateTimeEventOccurred { get; private set; }
		public TSource Sender { get; private set; }

		public DomainEvent(TSource sender)
		{
			if (sender == null)
				throw new ArgumentNullException(nameof(sender));

			DateTimeEventOccurred = DateTime.Now;
			Sender = sender;
		}
	}
}

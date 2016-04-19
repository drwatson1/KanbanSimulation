using KanbanSimulation.DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.DataSources
{
	class WorkInProgressViewDataSource
	{
		private readonly Func<int> GetWip;

		public WorkInProgressViewDataSource(Func<int> getWip)
		{
			if (getWip == null)
				throw new ArgumentNullException(nameof(getWip));
			GetWip = getWip;
		}

		public override string ToString()
		{
			return new String('.', GetWip());
		}
	}
}

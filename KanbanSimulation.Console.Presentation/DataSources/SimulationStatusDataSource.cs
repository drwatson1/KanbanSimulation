using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.DataSources
{
	class SimulationStatusDataSource
	{
		private readonly Func<bool> IsFinished;

		public SimulationStatusDataSource(Func<bool> isFinished)
		{
			if (isFinished == null)
				throw new ArgumentNullException(nameof(isFinished));

			IsFinished = isFinished;
		}

		public override string ToString()
		{
			return IsFinished() ? "Finished" : "Running";
		}
	}
}

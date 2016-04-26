using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.DataSources
{
	class CurrentWorkItemDataSource
	{
		private readonly Func<bool> HaveWi;
		private readonly Func<int> GetProgress;

		public CurrentWorkItemDataSource(Func<bool> haveWi, Func<int> getProgress)
		{
			if (getProgress == null)
				throw new ArgumentNullException(nameof(getProgress));
			if (haveWi == null)
				throw new ArgumentNullException(nameof(haveWi));

			HaveWi = haveWi;
			GetProgress = getProgress;
		}

		public override string ToString()
		{
			string t = String.Format("[{0}", HaveWi() ? "." : " ");
			if (HaveWi())
				t += $" ({GetProgress()})]";

			return t;
		}
	}
}

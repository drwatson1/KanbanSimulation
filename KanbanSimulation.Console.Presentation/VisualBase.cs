using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.ViewModel
{
	public class VisualBase
	{
		public Position Position { get; internal set; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public VisualBase()
		{
		}
	}
}

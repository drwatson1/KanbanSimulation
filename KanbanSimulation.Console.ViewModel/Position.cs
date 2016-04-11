using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.ViewModel
{
	public class Position
	{
		public readonly int Left;
		public readonly int Top;

		public Position(int left, int top)
		{
			Left = left;
			Top = top;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View
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

		public Position Adjust(int horiz, int vert)
		{
			return new Position(Left + horiz, Top + vert);
		}
	}
}

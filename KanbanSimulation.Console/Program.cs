using KanbanSimulation.Console.Forms;
using KanbanSimulation.Console.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var form = new SimulationForm("Test");

				form.Position = new Position(System.Console.CursorLeft, System.Console.CursorTop);

				var r = new ConsoleRenderer();
				form.Render(r);

				System.Console.CursorVisible = false;

				System.Console.ReadLine();
			}
			finally
			{
				System.Console.CursorVisible = true;
			}
		}
	}
}

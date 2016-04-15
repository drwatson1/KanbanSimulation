using KanbanSimulation.Console.Controllers;
using KanbanSimulation.Console.Forms;
using KanbanSimulation.Console.View;
using KanbanSimulation.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console
{
	class Program
	{
		static void Main()
		{
			try
			{
				var form = new SimulationForm("Test");

				form.Position = new Position(System.Console.CursorLeft, System.Console.CursorTop);

				var r = new ConsoleRenderer();

				var workProcess = WorkProcessFactory.CreateNoConstraintsWorkProcess();
				var sim = new Simulation(workProcess);

				var controller = new SimulationFormController(form, sim);

				System.Console.CursorVisible = false;

				form.Render(r);

				while(!sim.Tick())
				{
					sim.Tick();

					form.Render(r);
					System.Console.ReadLine();
				}

				System.Console.WriteLine("Simulation completed");
				System.Console.ReadLine();
			}
			finally
			{
				System.Console.CursorVisible = true;
			}
		}
	}
}

using KanbanSimulation.Console.Controllers;
using KanbanSimulation.Console.Forms;
using KanbanSimulation.Console.View;
using KanbanSimulation.Simulations;
using System.Threading;

namespace KanbanSimulation.Console
{
	internal class Program
	{
		private static void Main()
		{
			try
			{
				var workProcess = WorkProcessFactory.CreateNoConstraintsWorkProcess(5);
				var sim = new Simulation(workProcess);

				var form = new SimulationForm(workProcess.Name);

				form.Position = new Position(System.Console.CursorLeft, System.Console.CursorTop);

				var r = new ConsoleRenderer();

				var controller = new SimulationFormController(form, sim);

				System.Console.CursorVisible = false;

				form.Render(r);

				while (!sim.Tick())
				{
					controller.Tick();
					form.Render(r);
					Thread.Sleep(100);
					//System.Console.ReadLine();
				}

				form.Render(r);

				System.Console.WriteLine();
				System.Console.WriteLine();
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
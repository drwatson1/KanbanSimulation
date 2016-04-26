using BizArk.ConsoleApp;
using KanbanSimulation.Console.Controllers;
using KanbanSimulation.Console.Forms;
using KanbanSimulation.Console.View;
using KanbanSimulation.Simulations;

namespace KanbanSimulation.Console
{
	internal class Program
	{
		private static void Main()
		{
			BaCon.Start<Application>();
		}
	}
}
using BizArk.ConsoleApp;
using KanbanSimulation.Console.Controllers;
using KanbanSimulation.Console.Forms;
using KanbanSimulation.Console.View;
using KanbanSimulation.Simulations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System;

namespace KanbanSimulation.Console
{
	internal enum SimulationMode
	{
		Auto,
		Manual
	}

	//[CmdLineOptions("System")]
	internal class Application
		: BaseConsoleApp
	{
		public Application()
		{
			Bottleneck = 5;
			Limit = 1;
			Process = WorkProcessType.Push;
			Interval = 5;
			Mode = SimulationMode.Auto;
			Speed = 9;
		}

		[Required]
		[Description("Process type to use")]
		public WorkProcessType Process { get; set; }

		[Range(1, 10)]
		[CmdLineArg("b")]
		[Description("Bottleneck value for one of the operation")]
		public int Bottleneck { get; set; }

		[Range(1, 10)]
		[CmdLineArg("l")]
		[Description("Operations limit. Recommendation is to set this equals 3 for TOC and 1 for Kanban. It's not applicable for other processes")]
		public uint Limit { get; set; }

		[Range(1, 10)]
		[CmdLineArg("i")]
		[Description("Interval for measure avarage throughput. One interval equals full time of getting first work item to done (in ticks)")]
		public int Interval { get; set; }

		[CmdLineArg("m")]
		[Description("Simulation mode. In manual mode you need to press Enter for each step. In auto mode steps makes automatic. In this case you can set u p simulation speed")]
		public SimulationMode Mode { get; set; }

		[CmdLineArg("s")]
		[Range(1, 10)]
		[Description("Simulation speed in automatic mode. Set 1 for slowest")]
		public int Speed { get; set; }

		[Description("Make pause at the end of each state")]
		public bool Pause { get; set; }

		public override int Start()
		{
			SimulationForm form = null;
			try
			{
				var workProcess = WorkProcessFactory.CreateWorkProcess(Process, Bottleneck, Limit);

				var sim = new Simulation(workProcess);

				form = new SimulationForm(workProcess.Name, new ConsoleRenderer());

				form.Position = new Position(System.Console.CursorLeft, System.Console.CursorTop);

				var controller = new SimulationFormController(form, sim);

				System.Console.CursorVisible = false;

				form.Render();

				int currentState = 1;
				while (!sim.Tick())
				{
					controller.Tick();
					form.Render();

					if (Mode == SimulationMode.Manual)
					{
						form.SetStatus("Press Enter to do next step");
						System.Console.ReadLine();
						form.ResetStatus();
					}
					else
					{
						Thread.Sleep(((10 - Speed) + 1) * 100);
						if (Pause)
						{
							if (currentState != sim.CurrentWorkFlowState)
							{
								form.SetStatus("Press Enter to continue");
								System.Console.ReadLine();
								form.ResetStatus();
							}
						}
						currentState = sim.CurrentWorkFlowState;
					}
				}

				form.Render();
				form.SetStatus("Simulation completed");
			}
			finally
			{
				System.Console.CursorVisible = true;
			}

			return 0; // 0 for success.
		}

		public override bool Error(Exception ex)
		{
			System.Console.WriteLine(ex);
			return base.Error(ex);
		}
	}
}
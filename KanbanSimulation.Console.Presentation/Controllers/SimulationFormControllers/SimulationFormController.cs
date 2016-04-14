using KanbanSimulation.Console.Forms;
using KanbanSimulation.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.Controllers
{
	public class SimulationFormController
	{
		private readonly SimulationForm Form;
		private readonly Simulation Sim;

		public SimulationFormController(SimulationForm form, Simulation sim)
		{
			if (form == null)
				throw new ArgumentNullException(nameof(form));

			Form = form;

			BindAll();
		}

		private void BindAll()
		{
			
		}
	}
}

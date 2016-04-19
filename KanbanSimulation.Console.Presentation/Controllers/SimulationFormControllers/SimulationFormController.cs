using KanbanSimulation.Console.Forms;
using KanbanSimulation.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanSimulation.DomainModel.Interfaces;
using KanbanSimulation.Console.View;
using KanbanSimulation.Console.DataSources;

namespace KanbanSimulation.Console.Controllers
{
	public class SimulationFormController
	{
		private readonly SimulationForm Form;
		private readonly Simulation Sim;

		public SimulationFormController(SimulationForm form, Simulation sim)
		{
			if (sim == null)
				throw new ArgumentNullException(nameof(sim));
			if (form == null)
				throw new ArgumentNullException(nameof(form));

			Form = form;
			Sim = sim;

			BindAll();
		}

		private void BindAll()
		{
			Form.CurrentWorkInProgress.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.Process.WorkInProgress);
			Form.CompletedWorkItemsCount.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.Process.CompletedWorkItems);
			Form.CompletedWorkItemsProgress.DataSource = new WorkInProgressViewDataSource(() => Sim.Process.CompletedWorkItems);
			Form.LeadTime.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasuredLeadTime);
			Form.WorkInProgress.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasuredWorkInProgress);
			Form.Throughput.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasuredThroughput);

			BindOperations();
		}

		private void BindOperations()
		{
			foreach (var op in Sim.Process.Operations)
			{
				BindOperation(op);
			}
		}

		private void BindOperation(IOperation op)
		{
			var panel = Form.AddOperation(op.Id, op.Complexity);
			(panel.GetChildByName("wip") as TextBox).DataSource = DataSourceFactory.ObjectPropertyDataSource(() => op.WorkInProgress);
			(panel.GetChildByName("vis") as TextBox).DataSource = new WorkInProgressViewDataSource(() => op.WorkInProgress);
		}
	}
}

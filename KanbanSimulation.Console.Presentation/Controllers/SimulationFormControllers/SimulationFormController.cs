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

		public void Tick()
		{
			switch (Sim.CurrentWorkFlowState)
			{
				case 1:
					Form.Step1StackPanel.Visible = true;
					Form.Arrange();
					break;
				case 2:
					Form.Step2StackPanel.Visible = true;
					Form.Arrange();
					break;
				case 3:
					Form.Step3StackPanel.Visible = true;
					Form.Arrange();
					break;
			}
		}

		private void BindAll()
		{
			Form.MeasurementInterval.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasurementInterval);

			Form.CompletedWorkItemsCount.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.CompletedWorkItems);
			Form.CurrentWorkInProgress.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.WorkInProgress);
			Form.ElapsedTicks.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.ElapsedTicks);
			Form.CurrentStatus.DataSource = new SimulationStatusDataSource(() => Sim.IsFinished);
			Form.CurrentStep.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.CurrentWorkFlowState);

			Form.CompletedWorkItemsProgress.DataSource = new WorkInProgressViewDataSource(() => Sim.Process.CompletedWorkItems);

			Form.Step1LeadTime.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.FirstCycleLeadTime);

			Form.Step2WorkInProgress.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasuredWorkInProgress);
			Form.Step2Throughput.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasuredThroughput);

			Form.Step3LeadTime.DataSource = DataSourceFactory.ObjectPropertyDataSource(() => Sim.MeasuredLeadTime);

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

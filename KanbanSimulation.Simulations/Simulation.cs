using KanbanSimulation.Core;
using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System;
using System.Linq;

namespace KanbanSimulation.Simulations
{
	public class Simulation
	{
		private readonly IdGeneratorService identity = new IdGeneratorService();

		#region StateMachine and it's state variables

		private int CurrentTicks => Process.ElapsedTicks - FirstCycleElapsedTicks;
		private WorkItem EthalonWorkItem;
		private StateMachine Workflow;

		#endregion StateMachine and it's state variables

		#region Public properties

		#region Data after first workflow step

		public int FirstCycleLeadTime { get; private set; }
		public int FirstCycleElapsedTicks { get; private set; }

		#endregion Data after first workflow step

		#region Data after second workflow step

		public decimal MeasuredThroughput { get; private set; }      // Average count work items for 1 MeasurementInterval
		public int MeasuredWorkInProgress { get; private set; }      // WIP after first work item completed

		#endregion Data after second workflow step

		#region Data after third workflow step

		public int MeasuredLeadTime;            //  Ethalon work item lead time after it was done (after third step)

		#endregion Data after third workflow step

		#region Initial parameters

		public readonly WorkProcess Process;
		public readonly int MeasurementInterval;    // Throughput measure interval. 1 MeasurementInterval = FirstCycleElapsedTicks

		#endregion Initial parameters

		#region Current simulation state

		public int CurrentWorkFlowState => Workflow.CurrentStateId;
		public bool IsFinished => Workflow.IsFinished;
		public int ElapsedTicks => Process.ElapsedTicks;    // Elapsed ticks from begining
		public int WorkInProgress => Process.WorkInProgress;
		public int CompletedWorkItems => Process.CompletedWorkItems;	// Completed work items at all

		#endregion Current simulation state

		#endregion Public properties

		public Simulation(WorkProcess process, int measurementInterval = 5)
		{
			if (process == null)
				throw new System.ArgumentNullException(nameof(process));
			if (measurementInterval < 1)
				throw new ArgumentException("Should be greater 0", nameof(measurementInterval));

			Process = process;
			MeasurementInterval = measurementInterval;

			ConfigureStateMachine();
		}

		public bool Tick()
		{
			Workflow.NextStep();
			return Workflow.IsFinished;
		}

		#region Private methods

		private void ConfigureStateMachine()
		{
			var s3 = new State(() =>
			{
				if (EthalonWorkItem != null && OutputQueueContainsWorkItem(EthalonWorkItem))
					return true;

				if (EthalonWorkItem == null)
				{
					EthalonWorkItem = new WorkItem(identity.NextId());
					Process.Push(EthalonWorkItem);
				}

				Process.Done.Clear();
				Process.Tick(3);

				if (!OutputQueueContainsWorkItem(EthalonWorkItem))
					return false;

				MeasuredLeadTime = EthalonWorkItem.LeadTime;

				return true;
			}, 3);

			var s2 = new State(() =>
			{
				if (CurrentTicks >= FirstCycleElapsedTicks * MeasurementInterval)
					return true;

				if (Process.InputQueue.Count == 0)
					Process.Push(new WorkItem(identity.NextId()));

				Process.Tick(3);

				if (CurrentTicks < FirstCycleElapsedTicks * MeasurementInterval)
					return false;

				MeasuredThroughput = Process.Done.Count / ((decimal)CurrentTicks / FirstCycleElapsedTicks);
				MeasuredWorkInProgress = Process.WorkInProgress;

				Process.Done.Clear();

				return true;
			}, s3, 2);

			var s1 = new State(() =>
			{
				if (!Process.Done.Empty)
					return true;

				if (Process.InputQueue.Count == 0)
					Process.Push(new WorkItem(identity.NextId()));

				Process.Tick(3);

				if (Process.Done.Empty)
					return false;

				FirstCycleLeadTime = Process.Done[0].LeadTime;
				FirstCycleElapsedTicks = Process.ElapsedTicks;

				Process.Done.Clear();

				return true;
			}, s2, 1);

			var s0 = new State(() =>
			{
				if (Process.InputQueue.Count == 0)
					Process.Push(new WorkItem(identity.NextId()));

				Process.Tick();

				return true;
			}, s1, 0);

			Workflow = new StateMachine(s0, false);
		}

		private bool OutputQueueContainsWorkItem(WorkItem wi)
		{
			if (wi == null)
				return false;

			return Process.Done.Count(i => i.Equals(wi)) > 0;
		}

		#endregion Private methods
	}
}
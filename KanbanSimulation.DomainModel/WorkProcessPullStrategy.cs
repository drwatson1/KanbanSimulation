using KanbanSimulation.DomainModel.Interfaces;
using System;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcessPullStrategy : IWorkProcessStrategy
	{
		public void ConfigureInputQueue(Operation op, IInputQueue inputQueue)
		{
			op.InputQueue = inputQueue;
		}

		public void ConfigureOutputQueue(Operation op, IOutputQueue outputQueue)
		{
			op.OutputQueue = op.DoneQueue;
		}

		public void Push(Operation op, WorkItemQueue inputQueue, WorkItem wi)
		{
			inputQueue.Push(wi);
		}
	}
}

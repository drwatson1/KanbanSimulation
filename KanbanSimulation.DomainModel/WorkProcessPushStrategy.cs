using KanbanSimulation.DomainModel.Interfaces;
using System;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcessPushStrategy : IWorkProcessStrategy
	{
		public void ConfigureInputQueue(Operation op, IInputQueue inputQueue)
		{
			op.InputQueue = op.InProgressQueue;
		}

		public void ConfigureOutputQueue(Operation op, IOutputQueue outputQueue)
		{
			op.OutputQueue = outputQueue;
		}
	}
}
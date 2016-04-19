using KanbanSimulation.DomainModel.Interfaces;
using System;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcessPushStrategy : IWorkProcessStrategy
	{
		public void Push(WorkItem wi, IOutputQueue doneQueue, IOutputQueue outputQueue)
		{
			if (wi == null)
				throw new ArgumentNullException(nameof(wi));
			if (outputQueue == null)
				throw new ArgumentNullException(nameof(outputQueue));
			if (doneQueue == null)
				throw new ArgumentNullException(nameof(doneQueue));

			outputQueue.Push(wi);
		}

		// WorkItem already in inProgressQueue thus we don't need to pull
#pragma warning disable RECS0154 // Parameter is never used

		public void Pull(IInputQueue inputQueue, IOutputQueue inProgressQueue)
#pragma warning restore RECS0154 // Parameter is never used
		{
			if (object.ReferenceEquals(inputQueue, inProgressQueue) || inputQueue.Empty)
				return;

			inProgressQueue.Push(inputQueue.Pull());
		}
	}
}
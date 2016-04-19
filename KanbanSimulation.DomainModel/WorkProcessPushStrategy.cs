﻿using KanbanSimulation.DomainModel.Interfaces;
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

		public void Pull(IInputQueue inputQueue, IOutputQueue inProgressQueue)
		{
			if (object.ReferenceEquals(inputQueue, inProgressQueue) || inputQueue.Empty)
				return;

			inProgressQueue.Push(inputQueue.Pull());
		}
	}
}
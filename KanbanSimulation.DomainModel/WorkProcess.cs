using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanSimulation.DomainModel
{
	public class WorkProcess
	{
		private readonly List<Operation> operations = new List<Operation>();
		private readonly WorkItemQueue inputQueue = new WorkItemQueue();

		public WorkProcess()
		{

		}

		public IReadOnlyList<WorkItem> InputQueue => inputQueue.Items.ToList();

		public IReadOnlyList<Operation> Operations => operations;

		public WorkProcess AddOperation(Operation operation)
		{
			operations.Add(operation);

			return this;
		}

		public WorkProcess Push(WorkItem workItem)
		{
			inputQueue.Push(workItem);

			return this;
		}

		public WorkProcess Tick()
		{
			throw new NotImplementedException();
		}
	}
}
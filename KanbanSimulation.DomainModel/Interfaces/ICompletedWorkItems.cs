﻿using System.Collections.Generic;

namespace KanbanSimulation.DomainModel.Interfaces
{
	public interface ICompletedWorkItems : IReadOnlyList<IWorkItem>
	{
		bool Empty { get; }

		bool Contains(IWorkItem wi);

		void Remove(IWorkItem wi);

		void RemoveAt(int index);

		void Clear();
	}
}
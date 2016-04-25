﻿using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Simulations
{
	static public class WorkProcessFactory
	{
		private static readonly IdGeneratorService identity = new IdGeneratorService();

		static public WorkProcess CreateNoConstraintsPushWorkProcess(int bottleneck = 5)
		{
			if (bottleneck < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(bottleneck));

			return new WorkProcess($"Work process with out constraints (push, bottleneck={bottleneck})")
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(bottleneck, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()));
		}

		static public WorkProcess CreateNoConstraintsPullWorkProcess(int bottleneck = 5)
		{
			if (bottleneck < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(bottleneck));

			return new WorkProcess(new WorkProcessPullStrategy(), $"Work process with out constraints (pull, bottleneck={bottleneck})")
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(bottleneck, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()));
		}

		static public WorkProcess CreateKanbanSystem(int bottleneck = 5, uint limit = 1)
		{
			if (bottleneck < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(bottleneck));
			if (limit < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(limit));

			return new KanbanWorkProcess(limit, $"Kanban system (limit = {limit}, bottleneck={bottleneck})")
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(bottleneck, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()));
		}
	}
}

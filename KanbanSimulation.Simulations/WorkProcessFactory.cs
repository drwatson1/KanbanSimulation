using KanbanSimulation.DomainModel;
using KanbanSimulation.DomainModel.Services;
using System;

namespace KanbanSimulation.Simulations
{
	public enum WorkProcessType
	{
		Push,
		Pull,
		Kanban,
		Toc
	}

	static public class WorkProcessFactory
	{
		private static readonly IdGeneratorService identity = new IdGeneratorService();

		static public WorkProcess CreateWorkProcess(WorkProcessType type, int bottleneck = 5, uint? limit = null)
		{
			switch (type)
			{
				case WorkProcessType.Push:
					return CreateNoConstraintsPushWorkProcess(bottleneck);

				case WorkProcessType.Pull:
					return CreateNoConstraintsPullWorkProcess(bottleneck);

				case WorkProcessType.Kanban:
					{
						if (limit.HasValue)
							return CreateKanbanSystem(bottleneck, limit.Value);

						return CreateKanbanSystem(bottleneck);
					}
				case WorkProcessType.Toc:
					{
						if (limit.HasValue)
							return CreateTocSystem(bottleneck, limit.Value);

						return CreateTocSystem(bottleneck);
					}
				default:
					throw new ApplicationException("Unsupported process type");
			}
		}

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

		static public WorkProcess CreateTocSystem(int bottleneck = 5, uint limit = 3)
		{
			if (bottleneck < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(bottleneck));
			if (limit < 1)
				throw new ArgumentException("Must be 1 or greater", nameof(limit));

			var firstOp = new Operation(1, identity.NextId());

			var wp = new WorkProcess(new WorkProcessPullStrategy(), $"TOC system (limit = {limit}, bottleneck={bottleneck})")
				.AddOperation(firstOp)
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(bottleneck, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()))
				.AddOperation(new Operation(1, identity.NextId()));

			firstOp.Constraint = new WipLimitConstraint(wp.Operations[6], limit);

			return wp;
		}
	}
}
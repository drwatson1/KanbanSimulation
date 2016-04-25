using KanbanSimulation.DomainModel;
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

			var op1 = new Operation(1, identity.NextId());
			op1.Constraint = new WipConstraint(op1, limit);
			var op2 = new Operation(1, identity.NextId());
			op2.Constraint = new WipConstraint(op2, limit);
			var op3 = new Operation(1, identity.NextId());
			op3.Constraint = new WipConstraint(op3, limit);
			var op4 = new Operation(1, identity.NextId());
			op4.Constraint = new WipConstraint(op4, limit);
			var op5 = new Operation(1, identity.NextId());
			op5.Constraint = new WipConstraint(op5, limit);
			var op6 = new Operation(1, identity.NextId());
			op6.Constraint = new WipConstraint(op6, limit);
			var op7 = new Operation(bottleneck, identity.NextId());
			op7.Constraint = new WipConstraint(op7, limit);
			var op8 = new Operation(1, identity.NextId());
			op8.Constraint = new WipConstraint(op8, limit);
			var op9 = new Operation(1, identity.NextId());
			op9.Constraint = new WipConstraint(op9, limit);

			return new WorkProcess(new WorkProcessPullStrategy(), $"Kanban system (limit = {limit}, bottleneck={bottleneck})")
				.AddOperation(op1)
				.AddOperation(op2)
				.AddOperation(op3)
				.AddOperation(op4)
				.AddOperation(op5)
				.AddOperation(op6)
				.AddOperation(op7)
				.AddOperation(op8)
				.AddOperation(op9);
		}
	}
}

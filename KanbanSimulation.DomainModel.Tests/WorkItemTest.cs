using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KanbanSimulation.DomainModel.Tests
{
	[TestClass]
	public class WorkItemTest
	{
		[TestMethod]
		public void InitialLeadTimeMustBeZero()
		{
			WorkItem wi = new WorkItem(1);

			wi.LeadTime.Should().Be(0);
			wi.CurrentOperationProgress.Should().Be(0);
		}

		[TestMethod]
		public void LeadTimeIncrementedOnTick()
		{
			WorkItem wi = new WorkItem(1);

			wi.Tick();

			wi.LeadTime.Should().Be(1);
			wi.CurrentOperationProgress.Should().Be(1);
		}

		[TestMethod]
		public void NewOperationMustResetCurrentOperationProgress()
		{
			WorkItem wi = new WorkItem(1);

			wi.Tick();
			wi.StartNewOperation();

			wi.CurrentOperationProgress.Should().Be(0);
			wi.LeadTime.Should().Be(1);
		}


	}
}
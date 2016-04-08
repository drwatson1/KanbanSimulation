using KanbanSimulation.Core.Interfaces;
using StructureMap;

namespace KanbanSimulation.Core
{
	public static class IoC
	{
		static IoC()
		{
			Container = new Container(x =>
				{
					x.Scan(scan =>
					{
						scan.Assembly("KanbanSimulation.*");
						scan.WithDefaultConventions();
						scan.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
						scan.ConnectImplementationsToTypesClosing(typeof(IIdGeneratorService));
					});
				});
		}

		public static readonly IContainer Container;
	}
}
using KanbanSimulation.Core.Interfaces;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
					});
				});
		}

		public static readonly IContainer Container;
	}
}

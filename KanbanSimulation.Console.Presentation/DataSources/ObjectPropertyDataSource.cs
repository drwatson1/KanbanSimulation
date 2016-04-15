using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.DataSources
{
	public class ObjectPropertyDataSource<TValueType>
	{
		private readonly Func<TValueType> GetValue;

		public ObjectPropertyDataSource(Func<TValueType> getValue)
		{
			GetValue = getValue;
		}

		public override string ToString()
		{
			return GetValue().ToString();
		}
	}

	public static class DataSourceFactory
	{
		public static ObjectPropertyDataSource<T> ObjectPropertyDataSource<T>(Func<T> getValue)
		{
			return new ObjectPropertyDataSource<T>(getValue);
		}
	}
}

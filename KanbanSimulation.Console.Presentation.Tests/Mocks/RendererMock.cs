
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View.Tests
{
	class RendererMock : IRenderer
	{
		public string Text { get; protected set; }

		#region IRenderer implementation

		public Position Position { get; set; }

		public void Write(string text)
		{
			Text = text;
		}

		public void WriteLine(string text)
		{
			Text = text;
		}

		#endregion
	}
}

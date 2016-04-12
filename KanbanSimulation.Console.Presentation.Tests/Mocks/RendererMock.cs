
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View.Tests
{
	class RendererMock : IRenderer
	{
		private Position position;

		public string Text { get; protected set; }
		public int SetPositionCount;
		public int WriteCount;
		public int WriteLineCount;

		#region IRenderer implementation

		public Position Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
				++SetPositionCount;
			}
		}

		public void Write(string text)
		{
			Text = text;
			++WriteCount;
		}

		public void WriteLine(string text)
		{
			Text = text;
			++WriteLineCount;
		}

		#endregion
	}
}

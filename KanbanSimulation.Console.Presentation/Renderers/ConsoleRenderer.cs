using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View
{
	public class ConsoleRenderer : IRenderer
	{
		public Position Position
		{
			get
			{
				return new Position(System.Console.CursorLeft, System.Console.CursorTop);
			}

			set
			{
				if( value == null )
				{
					throw new ArgumentException("Position can't be null");
				}

				System.Console.SetCursorPosition(value.Left, value.Top);
			}
		}

		public void Write(string text)
		{
			System.Console.Write(text);
		}

		public void WriteLine(string text)
		{
			System.Console.WriteLine(text);
		}
	}
}

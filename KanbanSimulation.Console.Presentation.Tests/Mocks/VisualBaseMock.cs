using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View.Tests
{
	class VisualBaseMock : VisualBase
	{
		public int RenderCallCount;

		public VisualBaseMock(int width, int height)
			: base(width, height)
		{

		}

		public void SetWidth(int width)
		{
			Width = width;
		}

		public void SetHeight(int height)
		{
			Height = height;
		}

		public override void Render(IRenderer renderer)
		{
			++RenderCallCount;
		}
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View
{
	public class StackPanel: VisualBase
	{
		private readonly List<VisualBase> visuals = new List<VisualBase>();

		public readonly StackPanelOrientation Orientation;

		public StackPanel(int width, int height, StackPanelOrientation orientation)
			:	base(width, height)
		{
			Orientation = orientation;
		}

		public void AddChild(VisualBase child)
		{
			visuals.Add(child);

			if( Orientation == StackPanelOrientation.Horizontal )
			{
				Width += child.Width;
				if( Height < child.Height )
				{
					Height = child.Height;
				}
			}
		}

		public override void Render()
		{
			throw new NotImplementedException();
		}
	}
}

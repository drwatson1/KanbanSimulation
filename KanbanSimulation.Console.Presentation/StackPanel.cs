using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.ViewModel
{
	public class StackPanel: VisualBase
	{
		private List<VisualBase> visuals = new List<VisualBase>();

		public readonly StackPanelOrientation Orientation;

		public StackPanel(StackPanelOrientation orientation)
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
	}
}

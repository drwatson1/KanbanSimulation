using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.Console.View
{
	public class StackPanel: VisualBase
	{
		public readonly List<VisualBase> Childs = new List<VisualBase>();

		public readonly StackPanelOrientation Orientation;

		public StackPanel(StackPanelOrientation orientation)
			:	base(0, 0)
		{
			Orientation = orientation;
		}

		public StackPanel AddChild(VisualBase child)
		{
			Childs.Add(child);

			return this;
		}

		public bool ContainsChild(string name)
		{
			return Childs.Count(x => x.Name == name) > 0;
		}

		public VisualBase GetChildByName(string name)
		{
			return Childs.Single(x => x.Name == name);
		}

		public bool TryGetChildByName(string name, out VisualBase child)
		{
			child = Childs.SingleOrDefault(x => x.Name == name);
			return child != null;
		}

		public override void Arrange()
		{
			if (Orientation == StackPanelOrientation.Horizontal)
			{
				ArrangeChildsHorizontal();
			}
			else
			{
				ArrangeChildsVertical();
			}
		}

		private void ArrangeChildsVertical()
		{
			Width = 0;
			Height = 0;

			Position currentPosition = Position;

			foreach (var c in Childs)
			{
				c.Position = currentPosition;
				c.Arrange();

				Height += c.Height;
				if (Width < c.Width)
				{
					Width = c.Width;
				}

				currentPosition = currentPosition.Adjust(0, c.Height);
			}
		}

		private void ArrangeChildsHorizontal()
		{
			Width = 0;
			Height = 0;

			Position currentPosition = Position;

			foreach(var c in Childs)
			{
				c.Position = currentPosition;
				c.Arrange();

				Width += c.Width;
				if (Height < c.Height)
				{
					Height = c.Height;
				}
				
				currentPosition = currentPosition.Adjust(c.Width, 0);
			}
		}

		public override void Render(IRenderer renderer)
		{
			Childs.ForEach(x => x.Render(renderer));
		}
	}
}

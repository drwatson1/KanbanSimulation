using System;

namespace KanbanSimulation.Console.View
{
	public class TextBox : VisualBase
	{
		public object DataSource;

		public TextBox(int width, int height, object datasource = null)
			: base(width, height)
		{
			if (width <= 0)
			{
				throw new ArgumentException("Width must be > 0");
			}

			if (height != 1)
			{
				throw new ArgumentException("Height must be equals to 1");
			}

			DataSource = datasource;
		}

		public override void Render(IRenderer renderer)
		{
			if (renderer == null)
			{
				throw new ArgumentException("Renderer can't be null");
			}

			renderer.Position = Position;

			string text;
			string datasource = DataSource?.ToString();
			if (datasource == null)
				datasource = "";

			if (datasource.Length < Width)
			{
				string formatString = String.Format("{{0,-{0}}}", Width);
				text = String.Format(formatString, datasource);
			}
			else
			{
				text = datasource.Remove(Width, datasource.Length - Width);
			}

			renderer.Write(text);
		}
	}
}
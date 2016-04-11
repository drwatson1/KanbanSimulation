using System;

namespace KanbanSimulation.Console.View
{
	public sealed class TextBlock : VisualBase
	{
		public readonly object DataSource;
		public readonly IRenderer Renderer;

		public TextBlock(int width, int height, object datasource, IRenderer renderer)
			: base(width, height)
		{
			if (width <= 0)
			{
				throw new ArgumentException("Width must be > 0");
			}

			if (height <= 0)
			{
				throw new ArgumentException("Height must be > 0");
			}

			if (datasource == null)
			{
				throw new ArgumentException("DataSource can't be null");
			}
			DataSource = datasource;

			if ( renderer == null)
			{
				throw new ArgumentException("Renderer can't be null");
			}
			Renderer = renderer;
		}

		public override void Render()
		{
			Renderer.Position = Position;

			string text;
			string datasource = DataSource.ToString();
			if (datasource.Length < Width)
			{
				string formatString = String.Format("{{0,-{0}}}", Width);
				text = String.Format(formatString, DataSource);
			}
			else
			{
				text = datasource.Remove(Width, datasource.Length - Width);
			}

			Renderer.Write(text);
		}
	}
}
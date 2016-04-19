using System;

namespace KanbanSimulation.Console.View
{
	public abstract class VisualBase
	{
		private int width;
		private int height;
		private Position position = new Position(0, 0);
		public string Name { get; set; }
		public bool Visible { get; set; }

		public Position Position
		{
			get
			{
				return position;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentException("Position can't be null");
				}

				position = value;
			}
		}

		public int Width
		{
			get
			{
				if (!Visible)
					return 0;

				return width;
			}
			protected set
			{
				if (value < 0)
					throw new ArgumentException("Width must be >= 0");

				width = value;
			}
		}

		public int Height
		{
			get
			{
				if (!Visible)
					return 0;

				return height;
			}
			protected set
			{
				if (value < 0)
					throw new ArgumentException("Height must be >= 0");

				height = value;
			}
		}

		abstract public void Render(IRenderer renderer);

		virtual public void Arrange()
		{
		}

		protected VisualBase(int w, int h, string name = "")
		{
			if (w < 0)
				throw new ArgumentException("Width must be greater or equals to 0");
			if (h < 0)
				throw new ArgumentException("Height must be greater or equals to 0");

			Width = w;
			Height = h;
			Name = name;
			Visible = true;
		}
	}
}
using KanbanSimulation.Console.View;
using System;

namespace KanbanSimulation.Console.Forms
{
	public class SimulationForm
	{
		private StackPanel Root;

		public TextBox CurrentWorkInProgress;
		public StackPanel Operations;

		public TextBox CompletedWorkItemsCount;
		public TextBox CompletedWorkItemsProgress;

		public TextBox LeadTime;
		public TextBox WorkInProgress;
		public TextBox Throughput;

		public Position Position
		{
			get
			{
				return Root.Position;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentException("Position can't be null");
				}
				Root.Position = value;
				Root.Arrange();
			}
		}

		public SimulationForm(string caption)
		{
			Initialize(caption);
		}

		public void Render(IRenderer renderer)
		{
			Root.Render(renderer);
		}

		private void Initialize(string caption)
		{
			Root = new StackPanel(StackPanelOrientation.Vertical);

			Operations = new StackPanel(StackPanelOrientation.Vertical);

			Root.AddChild(new Label(caption))
				.AddChild(new Blank())
				.AddChild(LabelWithValue("Current WIP: ", ref CurrentWorkInProgress, 3))
				.AddChild(new Blank())
				.AddChild(Operations)
				.AddChild(new Blank())
				.AddChild(CompletedPanel())
				.AddChild(new Blank())
				.AddChild(LabelWithValue("Lead time:  ", ref LeadTime, 5))
				.AddChild(LabelWithValue("WIP:        ", ref WorkInProgress, 5))
				.AddChild(LabelWithValue("Throughput: ", ref Throughput, 5))
				;

			Root.Arrange();
		}

		public StackPanel AddOperation(int id, int complexity)
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);
			p.AddChild(new Label($"{id, -3}"))               // Id
				.AddChild(new Label($"({complexity})  ")) // complexity
				.AddChild(new TextBox(6, 1, null, "wip"))            // WIP
				.AddChild(new Label("["))            
				.AddChild(new TextBox(50, 1, null, "vis"))           // WIP visualization
				.AddChild(new Label("]"))
				;

			Operations.AddChild(p);

			Root.Arrange();

			return p;
		}

		private VisualBase CompletedPanel()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

			CompletedWorkItemsCount = new TextBox(3, 1);
			CompletedWorkItemsProgress = new TextBox(50, 1);

			p.AddChild(new Label("Completed: "))
				.AddChild(CompletedWorkItemsCount)
				.AddChild(new Label("["))
				.AddChild(CompletedWorkItemsProgress)
				.AddChild(new Label("]"))
				;

			return p;
		}

		private VisualBase LabelWithValue(string caption, ref TextBox value, int width)
		{
			var sp = new StackPanel(StackPanelOrientation.Horizontal);

			value = new TextBox(width, 1);

			sp.AddChild(new Label(caption))
				.AddChild(value);

			return sp;
		}
	}
}
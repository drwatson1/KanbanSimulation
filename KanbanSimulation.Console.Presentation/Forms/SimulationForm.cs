using KanbanSimulation.Console.View;
using System;

namespace KanbanSimulation.Console.Forms
{
	public class SimulationForm
	{
		private StackPanel Root;

		public TextBox MeasurementInterval;

		public TextBox CurrentWorkInProgress;
		public TextBox ElapsedTicks;
		public TextBox LastLeadTime;

		public TextBox CurrentStep;
		public TextBox CurrentStatus;

		public StackPanel Operations;

		public TextBox CompletedWorkItemsCount;
		public TextBox CompletedWorkItemsProgress;

		public StackPanel Step1StackPanel = new StackPanel(StackPanelOrientation.Vertical);
		public TextBox Step1LeadTime;

		public StackPanel Step2StackPanel = new StackPanel(StackPanelOrientation.Vertical);
		public TextBox Step2WorkInProgress;
		public TextBox Step2Throughput;

		public StackPanel Step3StackPanel = new StackPanel(StackPanelOrientation.Vertical);
		public TextBox Step3LeadTime;

		private TextBox Status = new TextBox(50, 1);
		private IRenderer Renderer;

		public void Arrange()
		{
			Root.Arrange();
		}

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

		public SimulationForm(string caption, IRenderer renderer)
		{
			if (renderer == null)
				throw new ArgumentNullException(nameof(renderer));

			Renderer = renderer;
			Initialize(caption);
		}

		public void SetStatus(string text)
		{
			Status.DataSource = text;
			Status.Render(Renderer);
		}

		public void ResetStatus()
		{
			Status.DataSource = null;
			Status.Render(Renderer);
		}

		public int Width => Root.Width;
		public int Height => Root.Height;

		public void Render()
		{
			Root.Render(Renderer);
		}

		private void Initialize(string caption)
		{
			Root = new StackPanel(StackPanelOrientation.Vertical);

			Operations = new StackPanel(StackPanelOrientation.Vertical);

			Root.AddChild(new Label(caption))
				.AddChild(new Blank())
				.AddChild(LabelWithValue("Measurement Interval: ", ref MeasurementInterval, 2))
				.AddChild(new Blank())
				.AddChild(Operations)
				.AddChild(new Blank())
				.AddChild(CompletedPanel())
				.AddChild(new Blank())
				.AddChild(LabelWithValue("Current WIP:    ", ref CurrentWorkInProgress, 3))
				.AddChild(LabelWithValue("Last lead time: ", ref LastLeadTime, 3))
				.AddChild(LabelWithValue("Elapsed ticks:  ", ref ElapsedTicks, 4))
				.AddChild(new Blank())
				.AddChild(LabelWithValue("Current step:   ", ref CurrentStep, 1))
				.AddChild(LabelWithValue("Status:         ", ref CurrentStatus, 9))
				.AddChild(new Blank())
				.AddChild(Step1StackPanel)
				.AddChild(new Blank())
				.AddChild(Step2StackPanel)
				.AddChild(new Blank())
				.AddChild(Step3StackPanel)
				.AddChild(new Blank())
				.AddChild(Status)
				;

			Step1StackPanel.Visible = false;
			Step1StackPanel.AddChild(new Label("Step 1:"));
			Step1StackPanel.AddChild(LabelWithValue("  Lead time:  ", ref Step1LeadTime, 5));

			Step2StackPanel.Visible = false;
			Step2StackPanel.AddChild(new Label("Step 2:"));
			Step2StackPanel.AddChild(LabelWithValue("  WIP:        ", ref Step2WorkInProgress, 5));
			Step2StackPanel.AddChild(LabelWithValue("  Throughput: ", ref Step2Throughput, 5));

			Step3StackPanel.Visible = false;
			Step3StackPanel.AddChild(new Label("Step 3:"));
			Step3StackPanel.AddChild(LabelWithValue("  Lead time:  ", ref Step3LeadTime, 5));

			Root.Arrange();
		}

		public StackPanel AddOperation(int id, int complexity)
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);
			p.AddChild(new Label($"{id,-3}"))               // Id
				.AddChild(new Label($"({complexity})  ")) // complexity
				.AddChild(new TextBox(4, 1, null, "wip"))            // WIP
				.AddChild(new TextBox(8, 1, null, "cur"))            // current work item
				.AddChild(CreateOperationQueues());

			Operations.AddChild(p);

			Root.Arrange();

			return p;
		}

		private VisualBase CreateOperationQueues()
		{
			return new StackPanel(StackPanelOrientation.Vertical, "Queues")
				.AddChild(CreateWipPanel("InProgress"))
				.AddChild(CreateWipPanel("Done"));
		}

		private StackPanel CreateWipPanel(string name)
		{
			return new StackPanel(StackPanelOrientation.Horizontal, name)
				.AddChild(new Label("["))
				.AddChild(new TextBox(100, 1, null, "vis"))
				.AddChild(new Label("]"));
		}

		private VisualBase CompletedPanel()
		{
			var p = new StackPanel(StackPanelOrientation.Horizontal);

			CompletedWorkItemsCount = new TextBox(3, 1);
			CompletedWorkItemsProgress = new TextBox(100, 1);

			p.AddChild(new Label("Completed: "))
				.AddChild(CompletedWorkItemsCount)
				.AddChild(new Label("      ["))
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
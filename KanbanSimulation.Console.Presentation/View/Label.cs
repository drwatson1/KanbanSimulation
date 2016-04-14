namespace KanbanSimulation.Console.View
{
	public class Label : TextBox
	{
		public Label(string text)
			: base(text.Length, 1, text)
		{
		}
	}
}
using Conversa.Runtime;
using Conversa.Runtime.Nodes.LogicalOperators;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class AbsoluteBoolNodeView : BaseNodeView<AbsoluteBoolNode>
	{
		protected override string Title => "Bool";

		public AbsoluteBoolNodeView(Conversation conversation) : base(new AbsoluteBoolNode(), conversation) { }

		public AbsoluteBoolNodeView(AbsoluteBoolNode data, Conversation conversation) : base(data, conversation) { }

		private Toggle toggle;

		protected override void SetBody()
		{
			bodyContainer.AddToClassList("p-5");
			toggle = new Toggle();
			toggle.SetValueWithoutNotify(Data.Value);
			toggle.RegisterValueChangedCallback(e =>
			{
				OnBeforeChange.Invoke();
				Data.Value = e.newValue;
			});
			bodyContainer.Add(toggle);
		}

		private static string FormatSelected(string a) => a;

		private static string FormatChoice(string choice) => choice;
	}
}
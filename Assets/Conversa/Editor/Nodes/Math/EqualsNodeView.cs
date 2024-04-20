using Conversa.Runtime;
using Conversa.Runtime.Nodes.MathOperators;

namespace Conversa.Editor.Nodes.Math
{
	public class EqualsNodeView : BaseNodeView<EqualsNode>
	{
		protected override string Title => "Equals";

		public EqualsNodeView(Conversation conversation) : base(new EqualsNode(), conversation) { }

		public EqualsNodeView(EqualsNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}
}
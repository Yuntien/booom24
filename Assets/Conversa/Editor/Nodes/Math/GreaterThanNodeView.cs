using Conversa.Runtime;
using Conversa.Runtime.Nodes.MathOperators;

namespace Conversa.Editor.Nodes.Math
{
	public class GreaterThanNodeView : BaseNodeView<GreaterThanNode>
	{
		protected override string Title => "Greater than";

		public GreaterThanNodeView(Conversation conversation) : base(new GreaterThanNode(), conversation) { }

		public GreaterThanNodeView(GreaterThanNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}
}
using Conversa.Runtime;
using Conversa.Runtime.Nodes.MathOperators;

namespace Conversa.Editor.Nodes.Math
{
	public class LessThanNodeView : BaseNodeView<LessThanNode>
	{
		protected override string Title => "Less than";

		public LessThanNodeView(Conversation conversation) : base(new LessThanNode(), conversation) { }

		public LessThanNodeView(LessThanNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}
}
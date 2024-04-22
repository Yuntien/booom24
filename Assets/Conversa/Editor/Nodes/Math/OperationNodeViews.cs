using Conversa.Runtime;
using Conversa.Runtime.Nodes.MathOperators;

namespace Conversa.Editor.Nodes.Math
{
	public class AddNodeView : BaseNodeView<AddNode>
	{
		protected override string Title => "Add";

		public AddNodeView(Conversation conversation) : base(new AddNode(), conversation) { }

		public AddNodeView(AddNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class SubtractNodeView : BaseNodeView<SubtractNode>
	{
		protected override string Title => "Subtract";

		public SubtractNodeView(Conversation conversation) : base(new SubtractNode(), conversation) { }

		public SubtractNodeView(SubtractNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class MultiplyNodeView : BaseNodeView<MultiplyNode>
	{
		protected override string Title => "Multiply";

		public MultiplyNodeView(Conversation conversation) : base(new MultiplyNode(), conversation) { }

		public MultiplyNodeView(MultiplyNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class DivideNodeView : BaseNodeView<DivideNode>
	{
		protected override string Title => "Divide";

		public DivideNodeView(Conversation conversation) : base(new DivideNode(), conversation) { }

		public DivideNodeView(DivideNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}
}
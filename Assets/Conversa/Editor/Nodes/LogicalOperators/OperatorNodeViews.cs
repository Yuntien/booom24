using Conversa.Runtime;

namespace Conversa.Editor
{
	public class AndNodeView : BaseNodeView<AndNode>
	{
		protected override string Title => "And";

		public AndNodeView(Conversation conversation) : base(new AndNode(), conversation) { }

		public AndNodeView(AndNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class NorNodeView : BaseNodeView<NorNode>
	{
		protected override string Title => "Nor";

		public NorNodeView(Conversation conversation) : base(new NorNode(), conversation) { }

		public NorNodeView(NorNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class OrNodeView : BaseNodeView<OrNode>
	{
		protected override string Title => "Or";

		public OrNodeView(Conversation conversation) : base(new OrNode(), conversation) { }

		public OrNodeView(OrNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class XorNodeView : BaseNodeView<XorNode>
	{
		protected override string Title => "Xor";

		public XorNodeView(Conversation conversation) : base(new XorNode(), conversation) { }

		public XorNodeView(XorNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }
	}

	public class NotNodeView : BaseNodeView<NotNode>
	{
		protected override string Title => "Not";

		public NotNodeView(Conversation conversation) : base(new NotNode(), conversation) { }

		public NotNodeView(NotNode data, Conversation conversation) : base(data, conversation) { }

		protected override void SetBody() { }

	}
}
using Conversa.Runtime;
using Conversa.Runtime.Nodes;

namespace Conversa.Editor
{
    public class AdvancedMessageNodeView : BaseNodeView<AdvancedMessageNode>
    {
        protected override string Title => "Advanced message";

        // Constructors

        public AdvancedMessageNodeView(Conversation conversation) : base(new AdvancedMessageNode(), conversation) { }

        public AdvancedMessageNodeView(AdvancedMessageNode data, Conversation conversation) : base(data, conversation) { }

        // Methods

        protected override void SetBody() { }
    }
}
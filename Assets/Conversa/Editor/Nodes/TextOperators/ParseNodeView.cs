using Conversa.Runtime;

namespace Conversa.Editor
{
    public class ParseNodeView : BaseNodeView<ParseNode>
    {
        protected override string Title => "Parse";

        public ParseNodeView(Conversation conversation) : base(new ParseNode(), conversation) { }

        public ParseNodeView(ParseNode data, Conversation conversation) : base(data, conversation) { }

        protected override void SetBody() { }
    }
}
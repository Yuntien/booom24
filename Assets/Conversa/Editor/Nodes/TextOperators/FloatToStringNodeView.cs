using Conversa.Runtime;

namespace Conversa.Editor
{
    public class FloatToStringNodeView : BaseNodeView<FloatToStringNode>
    {
        protected override string Title => "ToString";

        public FloatToStringNodeView(Conversation conversation) : base(new FloatToStringNode(), conversation) { }

        public FloatToStringNodeView(FloatToStringNode data, Conversation conversation) : base(data, conversation) { }

        protected override void SetBody() { }
    }
}
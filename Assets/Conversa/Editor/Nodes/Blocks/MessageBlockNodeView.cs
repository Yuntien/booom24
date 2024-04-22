using Conversa.Runtime;
using Conversa.Runtime.StackBlocks;

namespace Conversa.Editor
{
    public class MessageBlockNodeView : BaseNodeView<MessageBlockNode>, ILinearDialogueBlock
    {
        protected override string Title => "";

        public MessageBlockNodeView(MessageBlockNode data, Conversation conversation) : base(data, conversation) { }

        public MessageBlockNodeView(Conversation conversation) : base(new MessageBlockNode(), conversation) { }

        protected override void SetBody()
        {
            base.SetBody();
            titleContainer.RemoveFromHierarchy();
        }
    }
}
using Conversa.Runtime;
using Conversa.Runtime.StackBlocks;

namespace Conversa.Editor
{
    public class ActorBlockNodeView : BaseNodeView<ActorBlockNode>, ILinearDialogueBlock
    {
        protected override string Title => "";

        public ActorBlockNodeView(ActorBlockNode data, Conversation conversation) : base(data, conversation)
        {
        }

        public ActorBlockNodeView(Conversation conversation) : base(new ActorBlockNode(), conversation) { }

        protected override void SetBody()
        {
            base.SetBody();
            titleContainer.RemoveFromHierarchy();
        }
    }
}
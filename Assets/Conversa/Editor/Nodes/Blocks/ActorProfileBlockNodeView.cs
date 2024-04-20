using Conversa.Runtime;
using Conversa.Runtime.StackBlocks;

namespace Conversa.Editor
{
    public class ActorProfileBlockNodeView : BaseNodeView<ActorProfileBlockNode>, ILinearDialogueBlock
    {
        protected override string Title => "";

        public ActorProfileBlockNodeView(ActorProfileBlockNode data, Conversation conversation) : base(data, conversation) { }

        public ActorProfileBlockNodeView(Conversation conversation) : base(new ActorProfileBlockNode(), conversation) { }

        protected override void SetBody()
        {
            base.SetBody();
            titleContainer.RemoveFromHierarchy();
        }
    }
}
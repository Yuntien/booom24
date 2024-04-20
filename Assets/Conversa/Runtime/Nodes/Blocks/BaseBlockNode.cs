using Conversa.Runtime.Interfaces;

namespace Conversa.Runtime.StackBlocks
{
    
    public abstract class BaseBlockNode : BaseNode, IEventNode
    {
        public virtual void Process(Conversation conversation, ConversationEvents conversationEvents)
        {
            var blockNode = conversation.GetNextBlockNode(Guid);
            conversation.Process(blockNode, conversationEvents);
        }
    }
}
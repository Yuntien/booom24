namespace Conversa.Runtime.Interfaces
{
	public interface IEventNode : INode
	{
		void Process(Conversation conversation, ConversationEvents conversationEvents);
	}
}
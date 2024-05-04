using System;
using System.Linq;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using UnityEngine;

[Serializable]
[Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
[Port("Next", "next", typeof(BaseNode), Flow.Out, Capacity.One)]
public class GameControlNode : BaseNode, IEventNode
{
    // TEMPLATE: Replace with your own custom fields
    public string key;
    public string value;

    public GameControlNode() { }

    public void Process(Conversation conversation, ConversationEvents conversationEvents)
    {
        void Advance()
        {
            var nextNode = conversation.GetOppositeNodes(GetNodePort("next")).FirstOrDefault();
            conversation.Process(nextNode, conversationEvents);
        }

        // TEMPLATE: Replace with your own custom event
        var e = new GameControlEvent(key, value, Advance);
        conversationEvents.OnConversationEvent.Invoke(e);
    }
}

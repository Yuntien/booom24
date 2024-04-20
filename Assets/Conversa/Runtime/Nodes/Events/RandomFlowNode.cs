using System;
using System.Linq;
using Conversa.Runtime.Interfaces;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
    [Port("Next", "next", typeof(BaseNode), Flow.Out, Capacity.Many)]
    public class RandomFlowNode : BaseNode, IEventNode
    {
        public void Process(Conversation conversation, ConversationEvents conversationEvents)
        {
            var random = new Random();
            var nodes = conversation.GetOppositeNodes(GetNodePort("next")).ToList();
            var index = random.Next(nodes.Count);
            var node = nodes[index];
            conversation.Process(node, conversationEvents);
        }
    }
}
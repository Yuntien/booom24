using System;
using System.Linq;
using Conversa.Runtime.Interfaces;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
	[Port("Condition", "condition", typeof(bool), Flow.In, Capacity.One)]
	[Port("True", "true", typeof(BaseNode), Flow.Out, Capacity.One)]
	[Port("False", "false", typeof(BaseNode), Flow.Out, Capacity.One)]
	public class ConditionalNode : BaseNode, IEventNode
	{
		public void Process(Conversation conversation, ConversationEvents conversationEvents)
		{
			var condition = conversation.GetConnectedValueTo<bool>(this, "condition");
			var nextNode = conversation.GetOppositeNodes(GetNodePort(condition ? "true" : "false")).FirstOrDefault();
			conversation.Process(nextNode, conversationEvents);
		}
	}
}
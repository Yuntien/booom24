using System;
using System.Linq;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[Port("Next", "next", typeof(BaseNode), Flow.Out, Capacity.One)]
	public class BookmarkNode : BaseNode, IEventNode
	{
		[SerializeField] private string name = "New bookmark";

		public string Name
		{
			get => name;
			set => name = value;
		}

		public void Process(Conversation conversation, ConversationEvents conversationEvents)
		{
			var nextNode = conversation.GetOppositeNodes(GetNodePort("next")).FirstOrDefault();
			conversation.Process(nextNode, conversationEvents);
		}
	}
}
using System;
using System.Linq;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
	public class BookmarkJumpNode : BaseNode, IEventNode
	{
		[SerializeField] private string bookmarkName;
		[SerializeField] private string bookmarkGuid;

		public string BookmarkName
		{
			get => bookmarkName;
			set => bookmarkName = value;
		}

		public string BookmarkGuid
		{
			get => bookmarkGuid;
			set => bookmarkGuid = value;
		}
		
		// Methods

		public override bool IsValid(Conversation conversation) =>
			 conversation.AllNodes.ToList().Any(x => x.Guid == bookmarkGuid);

		public void Process(Conversation conversation, ConversationEvents conversationEvents)
		{
			var node = conversation.AllNodes.OfType<BookmarkNode>().FirstOrDefault(x => x.Name == bookmarkName);
			conversation.Process(node, conversationEvents);
		}
	}
}
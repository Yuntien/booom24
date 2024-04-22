using System;
using System.Linq;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Conversa.Runtime.Nodes
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]		
	[Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
	[Port("Next", "next", typeof(BaseNode), Flow.Out, Capacity.One)]
	public class EventNode : BaseNode, IEventNode
	{
		[SerializeField] private bool stopsFlow;
		
		[SerializeField]
		[FormerlySerializedAs("EventGuid")]
		private string eventGuid;
		
		[SerializeField]
		[FormerlySerializedAs("EventName")]
		private string eventName; // Redundant. Should be taken from the Properties list
		
		public string EventGuid { get => eventGuid; set => eventGuid = value; }
		public string EventName { get => eventName; set => eventName = value; }

		public EventNode(string eventGuid, string eventName, bool stopsFlow)
		{
			this.eventGuid = eventGuid;
			this.eventName = eventName;
			this.stopsFlow = stopsFlow;
		}
		
		public bool StopsFlow
		{
			get => stopsFlow;
			set => stopsFlow = value;
		}

		public void Process(Conversation conversation, ConversationEvents conversationEvents)
		{
			void Advance()
			{
				var nextNode = conversation.GetOppositeNodes(GetNodePort("next")).FirstOrDefault();
				conversation.Process(nextNode, conversationEvents);
			}
			
			var e = stopsFlow ? new UserEvent(EventName, stopsFlow, Advance) : new UserEvent(EventName, stopsFlow);

			conversationEvents.OnUserEvent.Invoke(e); // Deprecated
			conversationEvents.OnConversationEvent.Invoke(e);

			if (!stopsFlow) Advance();
		}

		public override bool IsValid(Conversation conversation) => conversation.PropertyExists(EventGuid);
	}
}
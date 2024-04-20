using System;
using System.Linq;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
	[Port("Next", "next", typeof(BaseNode), Flow.Out, Capacity.One)]
	public class MessageNode : BaseNode, IEventNode
	{
		public const string DefaultActor = "Actor name";
		public const string DefaultMessage = "Enter your message here";
		
		[SerializeField] private string actor = DefaultActor;
		[SerializeField] private Actor actorProfile;
		[SerializeField] private bool useActorProfile;
		[SerializeField] private string message = DefaultMessage;
		
		public string Actor
		{
			get => actor;
			set => actor = value;
		}

		public Actor ActorProfile
		{
			get => actorProfile;
			set => actorProfile = value;
		}

		public bool UseActorProfile
		{
			get => useActorProfile;
			set => useActorProfile = value;
		}

		public string Message
		{
			get => message;
			set => message = value;
		}
		
		public string ActorName => !UseActorProfile ? Actor : ActorProfile != null ? ActorProfile.DisplayName : "";

		public MessageNode() { }
		
		public MessageNode(Actor presetActor)
		{
			if (presetActor == null) return;
			useActorProfile = true;
			actorProfile = presetActor;
		}

		public MessageNode(bool useActorProfile)
		{
			UseActorProfile = useActorProfile;
		}

		public void Process(Conversation conversation, ConversationEvents conversationEvents)
		{
			void Advance()
			{
				var nodePort = GetNodePort("next");
				var oppositeNodes = conversation.GetOppositeNodes(nodePort); 
				var nextNode = oppositeNodes.FirstOrDefault();
				conversation.Process(nextNode, conversationEvents);
			}

			if (UseActorProfile)
			{
				var e = new ActorMessageEvent(actorProfile, message, Advance);
				conversationEvents.OnConversationEvent.Invoke(e);
			}
			else
			{
				var e = new MessageEvent(actor, message, Advance);
				conversationEvents.OnMessage.Invoke(e); // Deprecated
				conversationEvents.OnConversationEvent.Invoke(e);
			}
		}
	}
}
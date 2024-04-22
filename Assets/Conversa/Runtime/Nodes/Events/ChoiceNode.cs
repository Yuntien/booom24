using System;
using System.Collections.Generic;
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
	public class ChoiceNode : BaseNode, IEventNode
	{
		[SerializeField] private string actor;
		[SerializeField] private Actor actorProfile;
		[SerializeField] private bool useActorProfile;
		[SerializeField] private string message;
		[SerializeField]
		private List<PortDefinition<BaseNode>> options = new List<PortDefinition<BaseNode>>
		{
			new PortDefinition<BaseNode>("yes", "Yes"),
			new PortDefinition<BaseNode>("no", "No")
		};

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

		public List<PortDefinition<BaseNode>> Options
		{
			get => options;
			set => options = value;
		}

		public ChoiceNode() { }
		
		public ChoiceNode(Actor presetActor)
		{
			if (presetActor == null) return;
			useActorProfile = true;
			actorProfile = presetActor;
		}

		public ChoiceNode(bool useActorProfile)
		{
			UseActorProfile = useActorProfile;
		}

		public void Process(Conversation conversation, ConversationEvents conversationEvents)
		{
			void HandleClickOption(PortDefinition<BaseNode> portDefinition)
			{
				var nextNode = conversation.GetOppositeNodes(GetNodePort(portDefinition.Guid)).FirstOrDefault();
				conversation.Process(nextNode, conversationEvents);
			};

			Option NodePortToOption(PortDefinition<BaseNode> portDefinition) => new Option(portDefinition.Label, () => HandleClickOption(portDefinition));

			if (useActorProfile)
			{
				var choiceEvent = new ActorChoiceEvent(actorProfile, message, options.Select(NodePortToOption).ToList());
				conversationEvents.OnConversationEvent.Invoke(choiceEvent);
			}
			else
			{
				var choiceEvent = new ChoiceEvent(actor, message, options.Select(NodePortToOption).ToList());
				conversationEvents.OnChoice.Invoke(choiceEvent); // Deprecated
				conversationEvents.OnConversationEvent.Invoke(choiceEvent);
			}
		}

		public override bool ContainsPort(string portId, Flow flow)
		{
			if (base.ContainsPort(portId, flow)) return true;
			return flow == Flow.Out && options.Any(option => option.Guid == portId);
		}
	}
}
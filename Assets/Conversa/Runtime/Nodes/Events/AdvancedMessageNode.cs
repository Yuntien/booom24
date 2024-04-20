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
    public class AdvancedMessageNode : BaseNode, IEventNode
    {
        [SerializeField] private string actor;
        [SerializeField] private Actor actorProfile;
        [SerializeField] private string message;
        
        [Slot("Actor", "actor", Flow.In, Capacity.One)]
        public string Actor
        {
            get => actor;
            set => actor = value;
        }

        [Slot("Actor Profile", "actor-profile", Flow.In, Capacity.One)]
        public Actor ActorProfile
        {
            get => actorProfile;
            set => actorProfile = value;
        }

        [Slot("Message", "message", Flow.In, Capacity.One)]
        public string Message
        {
            get => message;
            set => message = value;
        }
        
        public void Process(Conversation conversation, ConversationEvents conversationEvents)
        {
            void Advance()
            {
                var nextNode = conversation.GetOppositeNodes(GetNodePort("next")).FirstOrDefault();
                conversation.Process(nextNode, conversationEvents);
            }

            var finalActorProfile = ProcessPort(conversation, "actor-profile", actorProfile);
            var finalActor = ProcessPort(conversation, "actor", actor);
            var finalMessage = ProcessPort(conversation, "message", message);

            if (finalActorProfile != null)
            {
                var actorMessageEvent = new ActorMessageEvent(finalActorProfile, finalMessage, Advance);
                conversationEvents.OnConversationEvent.Invoke(actorMessageEvent);
                return;
            }
            
            var messageEvent = new MessageEvent(finalActor, finalMessage, Advance);

            conversationEvents.OnMessage.Invoke(messageEvent); // Deprecated
            conversationEvents.OnConversationEvent.Invoke(messageEvent);
        }
    }
}
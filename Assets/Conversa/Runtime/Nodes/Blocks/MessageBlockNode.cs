using System;
using System.Linq;
using Conversa.Runtime.Events;
using UnityEngine;

namespace Conversa.Runtime.StackBlocks
{
    [Serializable]
    public class MessageBlockNode : BaseBlockNode
    {
        [SerializeField] private string message = "Test";
        
        [Slot("Message", "message", Flow.In, Capacity.One)]
        public string Message
        {
            get => message;
            set => message = value;
        }

        public override void  Process(Conversation conversation, ConversationEvents conversationEvents)
        {
            void Advance()
            {
                var blockNode = conversation.GetNextBlockNode(Guid);
                conversation.Process(blockNode, conversationEvents);
            }

            var actors = conversation.GetBlocksBefore(Guid).Select(conversation.GetNode)
                .Where(node => node is ActorBlockNode || node is ActorProfileBlockNode).ToList();

            var lastActor = actors.LastOrDefault();
            var processedMessage = GetProcessedMessage(conversation);
            
            switch (lastActor)
            {
                case ActorProfileBlockNode actorProfileBlockNode:
                {
                    var actorProfile = actorProfileBlockNode.GetActor(conversation);
                    var e1 = new ActorMessageEvent(actorProfile, processedMessage, Advance);
                    conversationEvents.OnConversationEvent.Invoke(e1);
                    break;
                }
                case ActorBlockNode actorBlockNode:
                {
                    var e2 = new MessageEvent(actorBlockNode.Actor, processedMessage, Advance);
                    conversationEvents.OnMessage.Invoke(e2); // Deprecated
                    conversationEvents.OnConversationEvent.Invoke(e2);
                    break;
                }
                case null:
                    var e3 = new MessageEvent("", processedMessage, Advance);
                    conversationEvents.OnMessage.Invoke(e3); // Deprecated
                    conversationEvents.OnConversationEvent.Invoke(e3);
                    break;
            }
        }

        private string GetProcessedMessage(Conversation conversation) =>
            conversation.IsConnected(Guid, "message")
                ? conversation.GetConnectedValueTo<string>(this, "message")
                : Message;
    }
}
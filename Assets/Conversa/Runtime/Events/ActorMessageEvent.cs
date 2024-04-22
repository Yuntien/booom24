using System;
using Conversa.Runtime.Interfaces;

namespace Conversa.Runtime.Events
{
    public class ActorMessageEvent : IConversationEvent
    {
        public Actor Actor { get; }
        public string Message { get; }
        public Action Advance { get; }

        public ActorMessageEvent(Actor actor, string message, Action advance)
        {
            Actor = actor;
            Message = message;
            Advance = advance;
        }
    }
}
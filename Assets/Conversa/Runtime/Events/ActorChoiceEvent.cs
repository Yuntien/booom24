using System.Collections.Generic;
using Conversa.Runtime.Interfaces;

namespace Conversa.Runtime.Events
{
    public class ActorChoiceEvent : IConversationEvent
    {
        public Actor Actor { get; }
        public string Message { get; }
        public List<Option> Options { get; }

        public ActorChoiceEvent(Actor actor, string message, List<Option> options)
        {
            Actor = actor;
            Message = message;
            Options = options;
        }
    }
}
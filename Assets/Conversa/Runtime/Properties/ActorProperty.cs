using System;
using Conversa.Runtime.Attributes;

namespace Conversa.Runtime.Properties
{
    [Serializable]
    [ConversationProperty("Actor", 0.68f, 0.49f, 0.20f)]
    public class ActorProperty : ValueProperty<Actor>
    {
        public ActorProperty(string name) : base(name) { }
    }
}
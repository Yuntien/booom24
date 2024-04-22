using System;
using Conversa.Runtime.Attributes;

namespace Conversa.Runtime.Properties
{
    [Serializable]
    [ConversationProperty("String", 0.48f, 0.49f, 0.90f)]
    public class StringProperty : ValueProperty<string>
    {
        public StringProperty(string name) : base(name) { }
    }
}
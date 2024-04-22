using System;
using Conversa.Runtime.Interfaces;

namespace Conversa.Runtime.Events
{
    public class UserEvent : IConversationEvent
    {
        public string Name { get; }
        public bool StopsFlow { get; }
        public Action Advance { get; }

        public UserEvent(string name, bool stopsFlow, Action advance)
        {
            Name = name;
            StopsFlow = stopsFlow;
            Advance = advance;
        }
        
        public UserEvent(string name, bool stopsFlow)
        {
            Name = name;
            StopsFlow = stopsFlow;
        }

    }
}
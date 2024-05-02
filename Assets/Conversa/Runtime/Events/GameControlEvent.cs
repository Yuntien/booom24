using System;
using Conversa.Runtime.Interfaces;

public class GameControlEvent : IConversationEvent
{
    // TEMPLATE: Replace with your custom fields 
    public string Message { get; }
    public Action Advance { get; }

    // TEMPLATE: Replace with your custom fields 
    public GameControlEvent(string message, Action advance)
    {
        Message = message;
        Advance = advance;
    }
}
using System;
using Conversa.Runtime.Interfaces;

public class GameControlEvent : IConversationEvent
{
    // TEMPLATE: Replace with your custom fields 
    public string Key { get; }
    public string Value { get; }
    public Action Advance { get; }

    // TEMPLATE: Replace with your custom fields 
    public GameControlEvent(string key, string value, Action advance)
    {
        Key = key;
        Value = value;
        Advance = advance;
    }
}
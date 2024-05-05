using System;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;

public class GameControlEvent : IConversationEvent
{
    // TEMPLATE: Replace with your custom fields 
    public string Key { get; }
    public string Value { get; }
    public Actor Actor { get; }
    public string ActorName { get; }
    public bool IsActorProfile { get; }
    public Action Advance { get; }

    // TEMPLATE: Replace with your custom fields 
    public GameControlEvent(string key, string value, Action advance, Actor actor, string actorName, bool isActorProfile)
    {
        Key = key;
        Value = value;
        Advance = advance;
        Actor = actor;
        ActorName = actorName;
        IsActorProfile = isActorProfile;
    }
}
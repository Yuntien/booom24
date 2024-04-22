using System;
using UnityEngine;

namespace Conversa.Runtime.StackBlocks
{
    [Serializable]
    public class ActorProfileBlockNode : BaseBlockNode
    {
        [SerializeField] private Actor actor;
        
        [Slot("Actor", "actor", Flow.In, Capacity.One)]
        public Actor Actor
        {
            get => actor;
            set => actor = value;
        }
        
        public Actor GetActor(Conversation conversation) =>
            conversation.IsConnected(Guid, "actor")
                ? conversation.GetConnectedValueTo<Actor>(this, "actor")
                : Actor;
    }
}
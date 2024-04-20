using System;
using UnityEngine;

namespace Conversa.Runtime.StackBlocks
{
    [Serializable]
    public class ActorBlockNode : BaseBlockNode
    {
        [SerializeField] private string actor;
        
        [Slot("Actor", "actor", Flow.In, Capacity.One)]
        public string Actor
        {
            get => actor;
            set => actor = value;
        }
    }
}
using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;

namespace Conversa.Runtime.unity_conversa.Runtime.Nodes.Literals
{
    [Serializable]
    [Port("Out", "out", typeof(Actor), Flow.Out, Capacity.Many)]
    public class ActorNode : BaseNode, IValueNode
    {
        [SerializeField] private Actor actor;
        public Actor Actor
        {
            get => actor;
            set => actor = value;
        }
        
        public T GetValue<T>(string portGuid, Conversation conversation) =>
            portGuid != "out" ? default : Converter.ConvertValue<Actor, T>(actor);
    }
}
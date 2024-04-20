using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes.LogicalOperators
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Out", "out", typeof(bool), Flow.Out, Capacity.Many)]
    public class AbsoluteBoolNode : BaseNode, IValueNode
    {
        [SerializeField] private bool value;

        public bool Value
        {
            get => value;
            set => this.value = value;
        }

        public override bool IsValid(Conversation conversation) => true;

        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            return portGuid != "out" ? default : Converter.ConvertValue<bool, T>(value);
        }
    }
}
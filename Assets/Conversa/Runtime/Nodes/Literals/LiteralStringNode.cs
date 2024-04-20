using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Out", "out", typeof(string), Flow.Out, Capacity.Many)]
    public class LiteralStringNode : BaseNode, IValueNode
    {
        [SerializeField] private string value;
        public string Value
        {
            get => value;
            set => this.value = value;
        }
        
        public override bool IsValid(Conversation conversation) => true;

        public T GetValue<T>(string portGuid, Conversation conversation) =>
            portGuid != "out" ? default : Converter.ConvertValue<string, T>(value);
    }
}
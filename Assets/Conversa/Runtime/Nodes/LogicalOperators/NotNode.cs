using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Out", "out", typeof(bool), Flow.Out, Capacity.Many)]
    public class NotNode : BaseNode, IValueNode
    {
        public const bool DefaultIn = false;
		
        [SerializeField] private bool @in = DefaultIn;
        
        [Slot("In", "in", Flow.In, Capacity.One)]
        public bool In
        {
            get => @in;
            set => @in = value;
        }
            
        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            if (portGuid != "out") return default;

            var input = ProcessPort(conversation, "in", @in);
            var output = !input;

            return Converter.ConvertValue<bool, T>(output);
        }
    }
}
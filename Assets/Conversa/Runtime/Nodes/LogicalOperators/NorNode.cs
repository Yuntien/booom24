using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Out", "out", typeof(bool), Flow.Out, Capacity.Many)]
    public class NorNode : BaseNode, IValueNode
    {
        public const bool DefaultInA = false;
        public const bool DefaultInB = false;
		
        [SerializeField] private bool inA = DefaultInA;
        [SerializeField] private bool inB = DefaultInB;
		
        [Slot("A", "in-a", Flow.In, Capacity.One)]
        public bool InA
        {
            get => inA;
            set => inA = value;
        }
		
        [Slot("B", "in-b", Flow.In, Capacity.One)]
        public bool InB
        {
            get => inB;
            set => inB = value;
        }

        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            if (portGuid != "out") return default;

            var condition1 = ProcessPort(conversation, "in-a", inA);
            var condition2 = ProcessPort(conversation, "in-b", inB);
			
            var output = !(condition1 || condition2);
			
            return Converter.ConvertValue<bool, T>(output);
        }
    }
}
using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes.MathOperators
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Out", "out", typeof(float), Flow.Out, Capacity.Many)]
    public class AddNode : BaseNode, IValueNode
    {
        public const float DefaultInA = 1;
        public const float DefaultInB = 1;

        [SerializeField] private float inA = DefaultInA;
        [SerializeField] private float inB = DefaultInB;
		
        [Slot("A", "in-a", Flow.In, Capacity.One)]
        public float InA
        {
            get => inA;
            set => inA = value;
        }
		
        [Slot("B", "in-b", Flow.In, Capacity.One)]
        public float InB
        {
            get => inB;
            set => inB = value;
        }

        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            if (portGuid != "out") return default;

            var value1 = ProcessPort(conversation, "in-a", inA);
            var value2 = ProcessPort(conversation, "in-b", inB);

            var output = value1 + value2;

            return Converter.ConvertValue<float, T>(output);
        }
    }
}
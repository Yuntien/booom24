using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Out", "out", typeof(bool), Flow.Out, Capacity.Many)]
    public class CompareNode : BaseNode, IValueNode
    {
        public enum CompareOperation
        {
            Greater,
            GreatOrEqual,
            Less,
            LessOrEqual,
            Equals,
            Different
        }
        
        public const float DefaultInA = 1;
        public const float DefaultInB = 0;

        [SerializeField] private CompareOperation operation;
        [SerializeField] private float inA = DefaultInA;
        [SerializeField] private float inB = DefaultInB;
        
        [Slot("A", "in-a", Flow.In , Capacity.One)]
        public float InA
        {
            get => inA;
            set => inA = value;
        }
        
        [Slot("B", "in-b", Flow.In , Capacity.One)]
        public float InB
        {
            get => inB;
            set => inB = value;
        }
        

        public CompareOperation Operation {
            get => operation;
            set => operation = value;
        }

        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            if (portGuid != "out") return default;

            var value1 = ProcessPort(conversation, "in-a", inA);
            var value2 = ProcessPort(conversation, "in-b", inB);
            
            var output = operation switch
            {
                CompareOperation.Greater => value1 > value2,
                CompareOperation.GreatOrEqual => value1 >= value2,
                CompareOperation.Less => value1 < value2,
                CompareOperation.LessOrEqual => value1 <= value2,
                CompareOperation.Equals => value1 == value2,
                CompareOperation.Different => value1 != value2,
                _ => false
            };

            return Converter.ConvertValue<bool, T>(output);
        }
    }}
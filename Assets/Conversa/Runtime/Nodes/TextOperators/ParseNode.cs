using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("Parameter", "parameter", typeof(string), Flow.In, Capacity.One, 4)]
    [Port("Out", "out", typeof(string), Flow.Out, Capacity.Many)]
    public class ParseNode : BaseNode, IValueNode
    {
        [SerializeField]
        private string value;
        
        [Slot("Value", "value", Flow.In, Capacity.One)]
        public string Value
        {
            get => value;
            set => this.value = value;
        }

        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            if (portGuid != "out") return default;

            var baseValue = ProcessPort(conversation, "value", Value);

            var parameter1 = conversation.GetConnectedValueTo<string>(this, "parameter 1");
            var parameter2 = conversation.GetConnectedValueTo<string>(this, "parameter 2");
            var parameter3 = conversation.GetConnectedValueTo<string>(this, "parameter 3");
            var parameter4 = conversation.GetConnectedValueTo<string>(this, "parameter 4");
            var sentence = string.Format(baseValue, parameter1, parameter2, parameter3, parameter4);

            return Converter.ConvertValue<string, T>(sentence);
        }
    }
}
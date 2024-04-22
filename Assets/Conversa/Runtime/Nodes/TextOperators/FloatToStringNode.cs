using System;
using Conversa.Runtime.Interfaces;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    [Port("In", "in", typeof(float), Flow.In, Capacity.One)]
    [Port("Out", "out", typeof(string), Flow.Out, Capacity.Many)]
    public class FloatToStringNode : BaseNode, IValueNode
    {
        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            var value = conversation.GetConnectedValueTo<float>(this, "in");
            var stringValue = value.ToString();
            return stringValue is T castedValue ? castedValue : default;
        }
    }
}
using System;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Conversa.Runtime.Nodes.PropertyNodes
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Serializable]
    public class PropertyNode : BaseNode, IValueNode
    {
        [SerializeField]
        [FormerlySerializedAs("PropertyGuid")]
        private string propertyGuid;

        [SerializeField]
        [FormerlySerializedAs("Name")]
        private string propertyName;

        public string PropertyGuid
        {
            get => propertyGuid;
            set => propertyGuid = value;
        }

        public string Name
        {
            get => propertyName;
            set => propertyName = value;
        }

        public PropertyNode(string propertyGuid, string propertyName)
        {
            PropertyGuid = propertyGuid;
            Name = propertyName;
        }
        
        public override bool IsValid(Conversation conversation) => conversation.PropertyExists(PropertyGuid);

        public T GetValue<T>(string portGuid, Conversation conversation)
        {
            var property = conversation.GetPropertyByGuid(PropertyGuid);
            return Converter.Convert<T>(property);
        }

        public override bool ContainsPort(string portId, Flow flow)
        {
            return base.ContainsPort(portId, flow) || (portId == "next" && flow == Flow.Out);
        }
    }
}
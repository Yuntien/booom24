using System;
using System.Linq;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using Conversa.Runtime.Properties;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace unity_conversa.Runtime.Nodes.PropertyNodes
{
    [MovedFrom(true, null, "Assembly-CSharp")]
    [Port("Previous", "previous", typeof(BaseNode), Flow.In, Capacity.Many)]
    [Port("Value", "new-float-value", typeof(float), Flow.In, Capacity.One)]
    [Port("Value", "new-string-value", typeof(string), Flow.In, Capacity.One)]
    [Port("Value", "new-bool-value", typeof(bool), Flow.In, Capacity.One)]
    [Port("Next", "next", typeof(BaseNode), Flow.Out, Capacity.One)]
    [Serializable]
    public class SetPropertyNode : BaseNode, IEventNode
    {
        [SerializeField]
        private string propertyGuid;
        public string PropertyGuid
        {
            get => propertyGuid;
            set => propertyGuid = value;
        }

        public void Process(Conversation conversation, ConversationEvents conversationEvents)
        {
            var property = conversation.GetProperty(PropertyGuid);
            switch (property)
            {
                case FloatProperty floatProperty:
                {
                    var newValue = conversation.GetConnectedValueTo<float>(this, "new-float-value");
                    floatProperty.Value = newValue;
                    break;
                }
                case StringProperty stringProperty:
                {
                    var newValue = conversation.GetConnectedValueTo<string>(this, "new-string-value");
                    stringProperty.Value = newValue;
                    break;
                }
                case BooleanProperty booleanProperty:
                {
                    var newValue = conversation.GetConnectedValueTo<bool>(this, "new-bool-value");
                    booleanProperty.Value = newValue;
                    break;
                }
            }

            var nextPort = GetNodePort("next");
            var nextNode = conversation.GetOppositeNodes(nextPort).FirstOrDefault();
            conversation.Process(nextNode, conversationEvents);
        }
    }
}
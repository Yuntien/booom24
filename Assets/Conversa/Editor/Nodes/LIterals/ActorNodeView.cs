using Conversa.Runtime;
using Conversa.Runtime.unity_conversa.Runtime.Nodes.Literals;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor.unity_conversa.Editor.Nodes.LIterals
{
    public class ActorNodeView: BaseNodeView<ActorNode>
    {
        protected override string Title => "Actor";

        public ActorNodeView(Conversation conversation) : base(new ActorNode(), conversation) { }

        public ActorNodeView(ActorNode data, Conversation conversation) : base(data, conversation) { }

        private ObjectField actorField;

        protected override void SetBody()
        {
            actorField = new ObjectField("Actor");
            actorField.objectType = typeof(Actor);
            actorField.SetValueWithoutNotify(Data.Actor);
            actorField.RegisterCallback<ChangeEvent<Object>>(e =>
            {
                switch (e.newValue)
                {
                    case null:
                        Data.Actor = null;
                        break;
                    case Actor actor:
                        Data.Actor = actor;
                        break;
                }
            });

            var wrapper = new VisualElement();
            wrapper.AddToClassList("p-5");
            wrapper.Add(actorField);

            bodyContainer.Add(wrapper);
        }
    }}
using Conversa.Runtime;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class GameControlNodeView : BaseNodeView<GameControlNode>
    {
        protected override string Title => "GameControl";

        protected TextField keyField;

        protected TextField valueField;

        protected TextField nameField;

        protected ActorField actorField;

        // Constructors

        public GameControlNodeView(Conversation conversation) : base(new GameControlNode(), conversation) { }

        public GameControlNodeView(GameControlNode data, Conversation conversation) : base(data, conversation) { }

        protected override void SetBody()
        {
            // TEMPLATE: Write your view body here
            keyField = new TextField();
            keyField.SetValueWithoutNotify(Data.key);
            keyField.RegisterValueChangedCallback(e => Data.key = e.newValue);
            keyField.isDelayed = true;

            valueField = new TextField();
            valueField.SetValueWithoutNotify(Data.value);
            valueField.RegisterValueChangedCallback(e => Data.value = e.newValue);
            valueField.isDelayed = true;

            actorField = new ActorField();
            actorField.SetValueWithoutNotify(Data.actorName, Data.isActorProfile, Data.actor);
            actorField.OnChange((actorName, isActorProfile, actor) => {
                Data.actorName = actorName;
                Data.isActorProfile = isActorProfile;
                Data.actor = actor;
            });

            bodyContainer.Add(actorField);
            bodyContainer.Add(keyField);
            bodyContainer.Add(valueField);
        }

        [NodeMenuModifier]
        private static void ModifyMenu(NodeMenuTree tree, Conversation conversation)
        {
            tree.AddMenuEntry<GameControlNodeView>("GameControl");
        }
    }
}

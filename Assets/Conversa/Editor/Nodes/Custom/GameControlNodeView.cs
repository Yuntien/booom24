using Conversa.Runtime;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class GameControlNodeView : BaseNodeView<GameControlNode>
    {
        protected override string Title => "GameControl";

        protected TextField messageField;

        // Constructors

        public GameControlNodeView(Conversation conversation) : base(new GameControlNode(), conversation) { }

        public GameControlNodeView(GameControlNode data, Conversation conversation) : base(data, conversation) { }

        protected override void SetBody()
        {
            // TEMPLATE: Write your view body here
            messageField = new TextField();
            messageField.SetValueWithoutNotify(Data.message);
            messageField.RegisterValueChangedCallback(e => Data.message = e.newValue);
            messageField.isDelayed = true;

            bodyContainer.Add(messageField);
        }

        [NodeMenuModifier]
        private static void ModifyMenu(NodeMenuTree tree, Conversation conversation)
        {
            tree.AddMenuEntry<GameControlNodeView>("GameControl");
        }
    }
}

using Conversa.Editor;
using Conversa.Runtime;
using UnityEngine.UIElements;

namespace unity_conversa.Editor.Nodes
{
    public class LiteralStringNodeView: BaseNodeView<LiteralStringNode>
    {
        protected override string Title => "String";

        public LiteralStringNodeView(Conversation conversation) : base(new LiteralStringNode(), conversation) { }

        public LiteralStringNodeView(LiteralStringNode data, Conversation conversation) : base(data, conversation) { }

        private TextField floatField;

        protected override void SetBody()
        {
            floatField = new TextField();
            floatField.SetValueWithoutNotify(Data.Value);
            floatField.RegisterValueChangedCallback(UpdateValue);
            floatField.isDelayed = true;

            var wrapper = new VisualElement();
            wrapper.AddToClassList("p-5");
            wrapper.Add(floatField);

            bodyContainer.Add(wrapper);
        }

        private void UpdateValue(ChangeEvent<string> e)
        {
            OnBeforeChange.Invoke();
            Data.Value = e.newValue;
        }        
    }
}
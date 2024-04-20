using System;
using Conversa.Runtime;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class CompareNodeView : BaseNodeView<CompareNode>
    {
        protected override string Title => "Compare number";

        public CompareNodeView(Conversation conversation) : base(new CompareNode(), conversation) { }

        public CompareNodeView(CompareNode data, Conversation conversation) : base(data, conversation) { }

        protected override void SetBody()
        {
            var field = new EnumField(CompareNode.CompareOperation.Equals) { value = Data.Operation };
            field.RegisterValueChangedCallback(HandleChangeOperation);
            bodyContainer.Add(field);
        }

        private void HandleChangeOperation(ChangeEvent<Enum> evt)
        {
            Data.Operation = (CompareNode.CompareOperation) evt.newValue;
        }
    }
}
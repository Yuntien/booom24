using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using UnityEditor;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public interface INodeInspector
    {
        VisualElement GetElement();
    }

    public abstract class BaseNodeInspector<T> : VisualElement, INodeInspector where T : INode
    {
        private Conversation conversation;
        protected T data;

        protected BaseNodeInspector(T data, Conversation conversation)
        {
            this.conversation = conversation;
            this.data = data;

            SetBody();
        }
        
        protected virtual void SetBody() { }

        public VisualElement GetElement() => this;

        protected void RegisterUndoStep(string message = "Conversa node update")
        {
            Undo.IncrementCurrentGroup();
            Undo.RegisterCompleteObjectUndo(conversation, message);

        }
    }
}
using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class BookmarkNodeInspector : BaseNodeInspector<BookmarkNode>
    {
        public BookmarkNodeInspector(BookmarkNode data, Conversation conversation) : base(data, conversation) { }

        private TextField bookmarkInput;

        protected override void SetBody()
        {
            var template = Resources.Load<VisualTreeAsset>("Inspectors/BookmarkNode");
            template.CloneTree(this);

            bookmarkInput = this.Q<TextField>();
            bookmarkInput.isDelayed = true;
            bookmarkInput.SetValueWithoutNotify(data.Name);
            bookmarkInput.RegisterValueChangedCallback(UpdateBookmark);
        }

        private void UpdateBookmark(ChangeEvent<string> e)
        {
            RegisterUndoStep();
            data.Name = e.newValue;
        }
    }
}
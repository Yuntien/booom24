using Conversa.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Conversa.Editor
{
    public class ConversaGroup : Group, IGroupable
    {
        private readonly GroupData data;

        public string Id => data.Guid;

        public ConversaGroup(GroupData data)
        {
            this.data = data;
            Setup();
        }

        private void Setup()
        {
            title = data.Title;
            SetPosition(new Rect(data.Position, Vector2.zero));
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            data.Title = newName;
        }

        public override bool AcceptsElement(GraphElement element, ref string reasonWhyNotAccepted)
        {
            switch (element)
            {
                case Group _:
                    reasonWhyNotAccepted = "Nested group is not supported yet.";
                    return false;
                case Scope _:
                    reasonWhyNotAccepted = "Nested scope is not supported yet.";
                    return false;
                default:
                    return true;
            }
        }

        public void UpdateData() => data.Position = GetPosition().position;
    }
}
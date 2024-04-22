using System.Collections.Generic;
using System.Linq;
using Conversa.Editor.Interfaces;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public class LinearDialogueView : StackNode, INodeContainer, INodeView, IGroupable
    {
        private LinearDialogueData data;

        private readonly Dictionary<string, Port> ports = new Dictionary<string, Port>();

        public string Guid => data.Guid;

        public string Id => Guid;

        public string GetPortId(Port port) => ports.First(x => x.Value == port).Key;

        public UnityEvent OnBeforeChange { get; }
        
        public bool GetPort(string guid, out Port port) => ports.TryGetValue(guid, out port);

        public void UpdateRect() { }

        public LinearDialogueView(LinearDialogueData data)
        {
            capabilities |= Capabilities.Groupable;
            this.data = data;
            Setup();
        }

        private void Setup()
        {
            SetPosition(new Rect(data.Position, GetPosition().size));

            AddPort(GetInputPort(), inputContainer, "previous");
            AddPort(GetOutputPort(), outputContainer, "next");
            
            headerContainer.Add(GetStackTitle());
        }

        protected override bool AcceptsElement(GraphElement element, ref int proposedIndex, int maxIndex) =>
            element is ILinearDialogueBlock;

        private VisualElement GetStackTitle() =>
            new Label("Linear dialogue") {style = {unityTextAlign = TextAnchor.MiddleCenter}};

        private void AddPort(Port port, VisualElement container, string id)
        {
            ports.Add(id, port);
            container.Add(port);
        }

        public void UpdateData() => data.Position = GetPosition().position;

        public void SetRealPosition(Vector2 position) { }

        public INode GetNode() => data;

        public void AddNode(INodeView view) => AddElement(view as Node);
        
        // Static 
        
        private static Port GetFlowPort(Orientation o, Direction d, Port.Capacity c) => new CustomPort(o, d, c, typeof(BaseNode)) {portName = ""};
        
        private static Port GetInputPort() => GetFlowPort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi);

        private static Port GetOutputPort() => GetFlowPort(Orientation.Vertical, Direction.Output, Port.Capacity.Single);

    }
}
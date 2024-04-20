using System.Linq;
using Conversa.Editor.Interfaces;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public enum SelectionState { None, One, Many }
    
    public class Inspector : VisualElement
    {
        private readonly ConversaGraphView graphView;
        private SelectionState selectionState;
        private string selectedNode;
        
        public Inspector(ConversaGraphView graphView)
        {
            this.graphView = graphView;
            selectionState = SelectionState.None;
            DrawProperties();
            graphView.schedule.Execute(Update).Every(100);
        }

        private void Draw(INodeView nodeView)
        {
            Clear();

            var node = nodeView.GetNode();
            if (node == null)
            {
                DrawProperties();
                return;
            };
            
            var inspector = General.GetInspector(node, graphView.conversation);
            
            if (inspector == null)
                DrawProperties();
            else
                Add(inspector.GetElement());
        }

        private void DrawProperties()
        {
            Clear();
           
            var propertyList = new PropertyList(graphView);
            var eventList = new EventList(graphView);

            var scrollView = new ScrollView();
            scrollView.Add(propertyList);
            scrollView.Add(eventList);
            Add(scrollView);
        }

        private void DrawMultipleNodes()
        {
            Clear();
            Add(new Label("Multiple nodes selected"));
        }

        private void Update(TimerState timerState)
        {
            var nodes = graphView.SelectedNodes().ToList();
            if (!nodes.Any())
            {
                if (selectionState == SelectionState.None) return;
                selectionState = SelectionState.None;
                selectedNode = string.Empty;
                DrawProperties();
                return;
            }
                
            if (nodes.Count > 1)
            {
                if (selectionState == SelectionState.Many) return;
                selectionState = SelectionState.Many;
                selectedNode = string.Empty;
                DrawMultipleNodes();
                return;
            }

            if (selectedNode == nodes.First().Guid) return;
            selectionState = SelectionState.One;
            var node = nodes.First();
            selectedNode = node.Guid;
            Draw(node);
        }
    }
}
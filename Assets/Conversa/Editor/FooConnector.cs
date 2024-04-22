using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Conversa.Editor
{
    public class FooConnector : IEdgeConnectorListener
    {
        private GraphViewChange graphViewChange;

        public void OnDropOutsidePort(Edge edge, Vector2 position) { }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            if (!(graphView is ConversaGraphView conversaGraphView)) return;
            
            var elementsToRemove = GetElementsToRemove(edge);

            if (elementsToRemove.Count > 0)
            {
                graphViewChange.elementsToRemove = elementsToRemove;
                graphView.DeleteElements(graphViewChange.elementsToRemove);
            }

            graphViewChange.edgesToCreate = new List<Edge> {edge};
            
            if (graphView.graphViewChanged != null)
                graphViewChange = graphView.graphViewChanged(graphViewChange);
            
            conversaGraphView.PaintEdge(General.ToEdgeData(edge));
        }

        private static List<GraphElement> GetElementsToRemove(Edge edge)
        {
            var elementsToRemove = new List<GraphElement>();

            if (edge.input.capacity == Port.Capacity.Single) 
                elementsToRemove.AddRange(edge.input.connections.Where(portEdge => portEdge != edge));
            if (edge.output.capacity == Port.Capacity.Single) 
                elementsToRemove.AddRange(edge.output.connections.Where(portEdge => portEdge != edge));

            return elementsToRemove;
        }
    }
}
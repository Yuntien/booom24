using UnityEditor.Experimental.GraphView;

namespace Conversa.Editor.Extensions
{
	public static class EdgeExtensions
	{
		public static bool IsConnectedTo(this Edge edge, Node node) =>
			edge.input.node == node || edge.output.node == node;
	}
}
using System;
using Conversa.Editor.Interfaces;
using Conversa.Runtime;
using Conversa.Runtime.Interfaces;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Conversa.Editor
{
	public static class General
	{
		// General

		public static string NewGuid() => Guid.NewGuid().ToString();

		// Ports

		private static CustomPort FlowPort(Direction direction, Port.Capacity capacity) =>
			new CustomPort(Orientation.Horizontal, direction, capacity, typeof(BaseNode));

		public static Port OutputFlowPort() => FlowPort(Direction.Output, Port.Capacity.Single);

		// Nodes

		public static Func<Port, bool> IsCompatible(Port port1) => port2 =>
			IsOppositeDirection(port1, port2) &&
			IsInDifferentNode(port1, port2) &&
			IsCompatibleType(port1, port2);

		private static bool IsInDifferentNode(Port p1, Port p2) => p1.node != p2.node;

		private static bool IsOppositeDirection(Port p1, Port p2) => p1.direction != p2.direction;

		private static bool IsCompatibleType(Port output, Port input) =>
			Converter.CanConvert(output.portType, input.portType);

		public static void SaveAsset(UnityEngine.Object target)
		{
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static T LoadAsset<T>(string guid) where T : UnityEngine.Object
		{
			var path = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<T>(path);

			return asset;
		}

		public static void ImportAsset(string assetGuid)
		{
			var path = AssetDatabase.GUIDToAssetPath(assetGuid);
			AssetDatabase.ImportAsset(path);
			AssetDatabase.ForceReserializeAssets(new[] { path });
		}

		public static EdgeData ToEdgeData(Edge edge)
		{
			var outputPort = edge.output;
			var inputPort = edge.input;
			
			if (!(edge.output.node is INodeView outputNode) || !(edge.input.node is INodeView inputNode))
				throw new Exception("Edge contains invalid data");
			
			var outputPortId = outputNode.GetPortId(outputPort);
			var inputPortId = inputNode.GetPortId(inputPort);

			return new EdgeData(
				new NodePort(outputNode.Guid, outputPortId),
				new NodePort(inputNode.Guid, inputPortId)
			);

		}

		public static TypeCache.TypeCollection GetNodeViews<T>() where T : INode =>
			TypeCache.GetTypesDerivedFrom<BaseNodeView<T>>();

		public static TypeCache.TypeCollection GetNodeInspectors<T>() where T : INode =>
			TypeCache.GetTypesDerivedFrom<BaseNodeInspector<T>>();

		public static INodeView GetView(INode node, Conversation conversation)
		{
			var nodeType = node.GetType();
			var method = typeof(General).GetMethod(nameof(GetNodeViews));
			var genericMethod = method.MakeGenericMethod(nodeType);
			var viewTypes = (TypeCache.TypeCollection)genericMethod.Invoke(null, null);

			if (viewTypes.Count < 1)
			{
				return null;
			}

			var nodeViewType = viewTypes[0];

			var args = new object[] { node, conversation };

			return Activator.CreateInstance(nodeViewType, args) as INodeView;
		}

		public static INodeInspector GetInspector(INode node, Conversation conversation)
		{
			var nodeType = node.GetType();
			var method = typeof(General).GetMethod(nameof(GetNodeInspectors));
			var genericMethod = method.MakeGenericMethod(nodeType);
			var inspectorsTypes = (TypeCache.TypeCollection)genericMethod.Invoke(null, null);

			if (inspectorsTypes.Count < 1)
			{
				return null;
			}

			var inspectorsType = inspectorsTypes[0];
			var args = new object[] { node, conversation };

			return Activator.CreateInstance(inspectorsType, args) as INodeInspector;
		}

		public static INodeView GetView(Type type, Conversation conversation)
		{
			object[] args = { conversation };
			return Activator.CreateInstance(type, args) as INodeView;
		}
	}
}
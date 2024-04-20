using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conversa.Editor.Interfaces;
using Conversa.Runtime;
using Conversa.Runtime.Attributes;
using Conversa.Runtime.Interfaces;
using Conversa.Runtime.Nodes.PropertyNodes;
using Conversa.Runtime.Properties;
using unity_conversa.Runtime.Nodes.PropertyNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class PropertyNodeView : BaseNodeView<PropertyNode>
	{
		public string PropertyGuid => Data.PropertyGuid;

		public PropertyNodeView(PropertyNode data, Conversation conversation) : base(data, conversation) { }

		private VisualElement MainContainer => outputContainer.Q<Label>().parent;

		protected override void SetPorts()
		{
			var property = Conversation.GetProperty(PropertyGuid);

			if (!(property is IValueProperty valueProperty)) return;

			var type = valueProperty.GetValueType();
			var port = new CustomPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, type);
			AddOutputPort(port, Data.Name, "next");
		}

		protected override void SetBody() => MainContainer.Add(Icons.Question);

		public void UpdateName(string newName) => outputContainer.Q<Label>().text = newName;

		public void UpdateColor(Color color) =>
			contentContainer.Q<VisualElement>("contents").style.backgroundColor = color;

		
		[NodeMenuModifier(2)]
		private static void ModifyMenu(NodeMenuTree tree, Conversation conversation)
		{
			tree.AddGroup("Properties");

			// "Set property" node

			SetPropertyNodeView Callback()
			{
				var node = new SetPropertyNode();

				var properties = conversation.GetValueProperties().ToList();
				
				if (properties.Count > 0) node.PropertyGuid = properties.First().Guid;

				var view = new SetPropertyNodeView(node, conversation);
				return view;
			}

			tree.AddMenuEntry("Set property", Callback, 2);
			
			// Properties
			
			var propertyTypes = TypeCache.GetTypesWithAttribute<ConversationPropertyAttribute>();

			foreach (var propertyType in propertyTypes)
			{
				var properties = GetProperties(conversation, propertyType);
				var propertyTypeName = GetPropertyTypeName(propertyType);

				if (properties.Count <= 0) continue;

				tree.AddGroup(propertyTypeName, 2);

				properties.ForEach(property =>
				{
					PropertyNodeView Callback()
					{
						var node = new PropertyNode(property.Guid, property.Name);
						var view = new PropertyNodeView(node, conversation);
						return view;
					}

					tree.AddMenuEntry(property.Name, Callback, 3);
				});
			}
		}

		private static string GetPropertyTypeName(System.Type type) =>
			type.GetCustomAttribute<ConversationPropertyAttribute>().PropertyName;

		private static List<BaseProperty> GetProperties(Conversation conversation, System.Type type) =>
			conversation.Properties.Where(x => x.GetType() == type).ToList();

		
		[DeletePropertyHandler]
		private static void HandleDeleteProperty(ConversaGraphView graphView, string propertyGuid)
		{
			var nodesToRemove = graphView
				.GetNodes<PropertyNodeView>()
				.Where(view => view.PropertyGuid == propertyGuid)
				.Cast<INodeView>()
				.ToList();
			
			graphView.DeleteNodes(nodesToRemove);
		}

		[EditPropertyHandler]
		private static void HandleEditProperty(ConversaGraphView graphView, BaseProperty property)
		{
			graphView
				.GetNodes<PropertyNodeView>()
				.Where(view => view.PropertyGuid == property.Guid)
				.ToList()
				.ForEach(view => view.UpdateName(property.Name));
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conversa.Editor.Interfaces;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class NodeSearchMenu : ScriptableObject, ISearchWindowProvider
	{
		private ConversaGraphView graphView;
		private ConversaEditorWindow editorWindow;

		public void Initialize(ConversaGraphView graphView, ConversaEditorWindow editorWindow)
		{
			this.graphView = graphView;
			this.editorWindow = editorWindow;
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			var nodeMenuTree = new NodeMenuTree(graphView.conversation);
			
			GetModifiers()
				.ToList()
				.OrderBy(info => info.GetCustomAttribute<NodeMenuModifierAttribute>().Position)
				.ToList()
				.ForEach(modifier => modifier.Invoke(null, new object[] { nodeMenuTree, graphView.conversation }));

			return nodeMenuTree.Tree;
		}

		public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			var position = ScreenToWorldMapper(context.screenMousePosition);

			if (searchTreeEntry.userData is Type viewType)
			{
				var view = General.GetView(viewType, graphView.conversation);
				view.SetRealPosition(position);
				graphView.AddNode(view);
			}

			if (searchTreeEntry.userData is Func<INodeView> generator)
			{
				var view = generator();
				view.SetRealPosition(position);
				graphView.AddNode(view);
			}

			return true;
		}

		private Vector2 ScreenToWorldMapper(Vector2 screenPosition)
		{
			var editorWindowPosition = screenPosition - editorWindow.WindowRect.position;
			var localPosition = graphView.contentViewContainer.WorldToLocal(editorWindowPosition);
			return localPosition;
		}

		// Static

		private static TypeCache.MethodCollection GetModifiers() =>
			TypeCache.GetMethodsWithAttribute<NodeMenuModifierAttribute>();
	}
}
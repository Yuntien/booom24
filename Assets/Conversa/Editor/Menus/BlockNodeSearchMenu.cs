using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class BlockNodeSearchMenu : ScriptableObject, ISearchWindowProvider
	{

		private ConversaGraphView graphView;
		private ConversaEditorWindow editorWindow;
		private LinearDialogueView stack;

		public LinearDialogueView Stack
		{
			get => stack;
			set => stack = value;
		}


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
				.OrderBy(info => info.GetCustomAttribute<StackNodeMenuModifierAttribute>().Position)
				.ToList()
				.ForEach(modifier => modifier.Invoke(null, new object[] {nodeMenuTree, graphView.conversation}));

			return nodeMenuTree.Tree;
		}

		public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			var position = ScreenToWorldMapper(context.screenMousePosition);
			if (searchTreeEntry.userData is Type viewType)
			{
				var view = General.GetView(viewType, graphView.conversation);
				view.SetRealPosition(position);
				graphView.AddNode(view, Stack);
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
			TypeCache.GetMethodsWithAttribute<StackNodeMenuModifierAttribute>();
	}
	
	// This attribute is used to add new entries into the "Add node" menu
	public class StackNodeMenuModifierAttribute : Attribute
	{
		public int Position { get; }

		public StackNodeMenuModifierAttribute(int position = 1)
		{
			Position = position;
		}
	}
}

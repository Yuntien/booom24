using System;
using System.Collections.Generic;
using Conversa.Editor.Interfaces;
using Conversa.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Conversa.Editor
{
	public class NodeMenuTree
	{
		public readonly List<SearchTreeEntry> Tree;
		private readonly Conversation conversation;

		public NodeMenuTree(Conversation conversation)
		{
			Tree = new List<SearchTreeEntry> { CreateGroupEntry("Add node", 0) };
			this.conversation = conversation;
		}

		private static SearchTreeEntry CreateGroupEntry(string name, int level = 1) =>
			new SearchTreeGroupEntry(new GUIContent(name), level);

		private static GUIContent CreateContent(string name) => new GUIContent(name, Texture2D.redTexture);

		private static SearchTreeEntry CreateEntry(string name, object userData, int level = 1) =>
			new SearchTreeEntry(CreateContent(name)) { userData = userData, level = level };

		// Sometimes the type of the nodeView is enough to create a new node view...
		public void AddMenuEntry<TView>(string name, int level = 1) =>
			Tree.Add(CreateEntry(name, typeof(TView), level));

		// ...but sometimes you need a function to know how to generate the node view
		public void AddMenuEntry<TView>(string name, Func<TView> createNodeView, int level) where TView : INodeView =>
			Tree.Add(CreateEntry(name, createNodeView, level));

		public void AddGroup(string groupName, int level = 1) =>
			Tree.Add(CreateGroupEntry(groupName, level));
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conversa.Editor.Extensions;
using Conversa.Editor.Interfaces;
using Conversa.Runtime;
using Conversa.Runtime.Attributes;
using Conversa.Runtime.Interfaces;
using Conversa.Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class ConversaGraphView : SuperGraphView, INodeContainer
	{
		public Conversation conversation;
		private readonly NodeSearchMenu nodeSearchMenu;
		private readonly BlockNodeSearchMenu blockNodeSearchMenu;
		private readonly List<ConversaNote> notes = new List<ConversaNote>();
		private readonly List<ConversaGroup> groups = new List<ConversaGroup>();
		private readonly List<LinearDialogueView> stacks = new List<LinearDialogueView>();

		// Constructor

		public ConversaGraphView(ConversaEditorWindow editorWindow, Conversation conversation)
		{
			nodeSearchMenu = ScriptableObject.CreateInstance<NodeSearchMenu>();
			nodeSearchMenu.Initialize(this, editorWindow);

			blockNodeSearchMenu = ScriptableObject.CreateInstance<BlockNodeSearchMenu>();
			blockNodeSearchMenu.Initialize(this, editorWindow);

			OnNodeCreationRequest.AddListener(HandleNodeCreationRequest);
			OnMoveElements.AddListener(HandleMoveElements);
			OnDeleteElements.AddListener(HandleRemoveElements);
			OnNewElements.AddListener(HandleNewElements);

			OnElementsAddedToGroup.AddListener(HandleElementsAddedToGroup);
			OnElementsRemovedFromGroup.AddListener(HandleElementsRemovedFromGroup);

			OnElementsAddedToStack.AddListener(HandleElementsAddedToStack);
			OnElementsRemovedFromStack.AddListener(HandleElementsRemovedFromStack);

			serializeGraphElements = Handlers.HandleSerializeGraphElements;
			unserializeAndPaste = Handlers.HandleUnserializeAndPaste;
			canPasteSerializedData = Handlers.HandleCanPasteSerializedData;
			
			unserializeAndPaste = null;
			deleteSelection = null;

			Load(conversation);
		}
		
		private void HandleElementsAddedToStack(StackNode stack, int position, IEnumerable<GraphElement> elements)
		{
			var addedGuids = elements.OfType<INodeView>().Select(x => x.Guid);

			if (stack is LinearDialogueView linearDialogueView &&
			    linearDialogueView.GetNode() is LinearDialogueData data)
				data.Blocks.InsertRange(position, addedGuids);
		}

		private static void HandleElementsRemovedFromStack(StackNode stack, IEnumerable<GraphElement> elements)
		{
			var removedGuids = elements.OfType<INodeView>().Select(x => x.Guid);

			if (stack is LinearDialogueView linearDialogueView &&
			    linearDialogueView.GetNode() is LinearDialogueData data)
				data.Blocks.RemoveAll(x => removedGuids.Contains(x));
		}

		// Handlers


		private void HandleNodeChange()
		{
			Undo.IncrementCurrentGroup(); // Footnote #2
			Undo.RegisterCompleteObjectUndo(conversation, "Conversa node update");
		}

		private void HandleNodeCreationRequest(NodeCreationContext context)
		{
			if (context.target is LinearDialogueView linearDialogueView)
			{
				blockNodeSearchMenu.Stack = linearDialogueView;
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), blockNodeSearchMenu);
			}
			else
			{
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), nodeSearchMenu);
			}
		}

		private void HandleMoveElements(GraphViewChange change)
		{
			Undo.RegisterCompleteObjectUndo(conversation, "Graph element moved");
			change.movedElements?.OfType<INodeView>().ToList().ForEach(HandleNodeMoved);
			change.movedElements?.OfType<ConversaNote>().ToList().ForEach(HandleNoteMoved);
			change.movedElements?.OfType<ConversaGroup>().ToList().ForEach(HandleGroupMoved);
			change.movedElements?.OfType<LinearDialogueView>().ToList().ForEach(HandleStackMoved);
		}

		private void HandleNodeMoved(INodeView nodeView) { nodeView.UpdateRect(); }

		private void HandleNoteMoved(ConversaNote note) { conversation.GetNote(note.Id).Rect = note.layout; }

		private void HandleGroupMoved(ConversaGroup group)
		{
			// Moving the group does not trigger moving the nodes inside.
			// We have to invoke it manually
			foreach (var element in group.containedElements.OfType<IGroupable>())
				element.UpdateData();

			// The position is controlled by the elements inside EXCEPT when it is empty.
			// Therefore, we need this
			group.UpdateData();
		}

		private void HandleStackMoved(LinearDialogueView view)
		{
			view.UpdateData();
		}

		private void HandleRemoveElements(GraphViewChange change)
		{
			// Undo.RegisterCompleteObjectUndo(conversation, "Graph element removed");

			change.elementsToRemove?.ForEach(el =>
			{
				switch (el)
				{
					case Edge edge:
						conversation.RemoveEdge(edge.userData as EdgeData);
						break;
					case ConversaNote note:
						conversation.RemoveNote(note.Id);
						break;
					case INodeView baseNodeView:
						conversation.RemoveNode(baseNodeView.Guid);
						break;
					case ConversaGroup group:
						conversation.Groups.RemoveAll(x => x.Guid == group.Id);
						break;
				}
			});
		}

		private void HandleNewElements(GraphViewChange change)
		{
			Undo.RegisterCompleteObjectUndo(conversation, "Graph edge added");
			change.edgesToCreate?.ForEach(edge => conversation.AddEdge(General.ToEdgeData(edge)));
		}

		private void HandleCreateNote(Vector2 worldPosition)
		{
			var position = contentViewContainer.WorldToLocal(worldPosition);
			var data = new StickyNoteData(position);
			PaintNote(data);
			conversation.StickyNotes.Add(data);
		}

		private void HandleGroupElements()
		{
			var data = new GroupData("New group");
			foreach (var item in selection.OfType<IGroupable>())
				data.Elements.Add(item.Id);

			PaintGroup(data);

			conversation.Groups.Add(data);
		}

		private void HandleElementsAddedToGroup(Group group, IEnumerable<GraphElement> elements)
		{
			if (!(group is ConversaGroup conversaGroup)) return;
			var groupData = conversation.GetGroup(conversaGroup.Id);
			foreach (var groupable in elements.OfType<IGroupable>())
				groupData.Elements.Add(groupable.Id);
		}

		private void HandleElementsRemovedFromGroup(Group group, IEnumerable<GraphElement> elements)
		{
			if (!(group is ConversaGroup conversaGroup)) return;
			var groupData = conversation.GetGroup(conversaGroup.Id);
			if (groupData == null) return;
			foreach (var groupable in elements.OfType<IGroupable>())
				groupData.Elements.Remove(groupable.Id);
		}

		// Other

		private void Load(Conversation newConversation)
		{
			if (newConversation == null)
			{
				Debug.LogError("Error: Could not load conversation. Null!");
				return;
			}

			conversation = newConversation;
			conversation.Validate();

			PaintStartNode(conversation.StartNode);
			conversation.UserNodes.ToList().ForEach(PaintNode);
			conversation.StickyNotes.ForEach(PaintNote);
			conversation.Groups.ForEach(PaintGroup);
			conversation.Edges.ForEach(PaintEdge);
			conversation.LinearDialogueNodes.ToList().ForEach(linearDialogue =>
			{
				if (GetGraphElement(linearDialogue.Guid) is LinearDialogueView view)
				{
					linearDialogue.Blocks.Select(GetGraphElement).ToList().ForEach(block =>
					{
						view.AddElement(block);
					});
				}
			});
		}

		// Add

		private void PaintNote(StickyNoteData data)
		{
			var stickyNote = new ConversaNote(data);
			AddElement(stickyNote);
			notes.Add(stickyNote);
		}

		private void PaintGroup(GroupData data)
		{
			var group = new ConversaGroup(data);
			var elements = data.Elements.Select(GetGraphElement).Where(x => x != null);
			group.AddElements(elements);
			groups.Add(group);
			Add(group);
		}

		private void PaintStartNode(BookmarkNode data)
		{
			var node = new BookmarkNodeView(data, conversation);
			node.SetPermanent();
			node.OnBeforeChange.AddListener(HandleNodeChange);
			AddElement(node);
		}

		private void PaintNode(INode node)
		{
			if (node is LinearDialogueData linearDialogueData)
				PaintNode(new LinearDialogueView(linearDialogueData));
			else
				PaintNode(General.GetView(node, conversation));
		}

		private void PaintNode(INodeView nodeView)
		{
			switch (nodeView)
			{
				case null:
					Debug.LogWarning("Painting error: Unable to paint node");
					return;

				case PropertyNodeView propertyNodeView:
					{
						var property = conversation.GetProperty(propertyNodeView.PropertyGuid);
						var color = property.GetType().GetCustomAttribute<ConversationPropertyAttribute>().Color;
						propertyNodeView.UpdateName(property.Name);
						propertyNodeView.UpdateColor(color);
						break;
					}
			}

			AddElement(nodeView as Node);
		}

		public void AddNode(INodeView nodeView)
		{
			PaintNode(nodeView);
			conversation.AddNode(nodeView.GetNode());
		}

		public void AddNode(INodeView childView, LinearDialogueView parentView)
		{
			var parentData = parentView.GetNode() as LinearDialogueData;
			var childData = childView.GetNode() ;

			parentView.AddElement(childView as Node);

			conversation.AddNode(childView.GetNode());
			parentData.Blocks.Add(childData.Guid);
		}

		public void PaintEdge(EdgeData edgeData)
		{
			GetPorts(edgeData, out var outputPort, out var inputPort);

			if (outputPort == null || inputPort == null)
			{
				Debug.LogWarning("Paint error: Invalid edge");
				return;
			}

			var edge = new Edge { output = outputPort, input = inputPort };
			inputPort.Connect(edge);
			outputPort.Connect(edge);
			edge.userData = edgeData;
			AddElement(edge);
		}

		public void DeleteNodes(List<INodeView> nodesToDelete)
		{
			// get edges

			var edgesToRemove = new List<Edge>();

			foreach (var node in nodesToDelete)
				edgesToRemove.AddRange(GetDependantEdges(node as Node).ToList());

			// Collect in a list

			var elementsToRemove = nodesToDelete.Cast<GraphElement>().ToList();
			elementsToRemove.AddRange(edgesToRemove);

			// Update data

			DeleteElements(elementsToRemove);
		}

		// Getters

		private IEnumerable<Edge> GetDependantEdges(Node node) =>
			edges.ToList().Where(edge => edge.IsConnectedTo(node));

		private GraphElement GetGraphElement(string id)
		{
			var node = nodes.ToList().OfType<INodeView>().FirstOrDefault(x => x.Guid == id);
			if (node != null)
				return node as GraphElement;

			var note = notes.ToList().FirstOrDefault(x => x.Id == id);
			return note;
		}

		public IEnumerable<T> GetNodes<T>() => nodes.ToList().OfType<T>();

		private INodeView GetNode(string guid)
		{
			var node = nodes.ToList().Cast<INodeView>().ToList().Find(x => x.Guid == guid);
			if (node != null) return node;

			var stack = stacks.ToList().Cast<INodeView>().ToList().Find(x => x.Guid == guid);
			return stack;
		}

		private (INodeView, INodeView) GetNodesFromEdge(EdgeData data) => (GetNode(data.Output), GetNode(data.Input));

		private INodeView GetNode(NodePort nodePort) => GetNode(nodePort.Node);

		private void GetPorts(EdgeData edgeData, out Port outputPort, out Port inputPort)
		{
			var (outputNode, inputNode) = GetNodesFromEdge(edgeData);
			if (outputNode == null || inputNode == null)
			{
				outputPort = null;
				inputPort = null;
				return;
			}

			if (!outputNode.GetPort(edgeData.Output.Port, out outputPort))
				Debug.LogWarning("Output port not found");

			if (!inputNode.GetPort(edgeData.Input.Port, out inputPort))
				Debug.LogWarning("Input port not found");
		}

		// Delete

		public override EventPropagation DeleteSelection()
		{
			Handlers.ExecuteDeleteNodes(new object[] { this });
			return base.DeleteSelection();
		}

		// Other

		public IEnumerable<INodeView> SelectedNodes() => selection.OfType<INodeView>();

		public override List<Port> GetCompatiblePorts(Port port, NodeAdapter nodeAdapter) =>
			ports.ToList().Where(General.IsCompatible(port)).ToList();

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			base.BuildContextualMenu(evt);

			// This needs to be cached at this point in a variable. Cannot be read inside the callback
			var mousePosition = evt.mousePosition;


			if (evt.target is GraphView)
			{
				evt.menu.InsertAction(1, "Create Sticky Note", e => HandleCreateNote(mousePosition));
				evt.menu.InsertAction(2, "Create Node Stack", e => HandleCreateStack(mousePosition));
			}

			if (evt.target is IGroupable groupable && !conversation.IsGrouped(groupable.Id))
				evt.menu.InsertAction(1, "Group nodes", e => HandleGroupElements());
		}

		private void HandleCreateStack(Vector2 worldPosition)
		{
			var position = contentViewContainer.WorldToLocal(worldPosition);
			var data = new LinearDialogueData(position);
			var view = new LinearDialogueView(data);
			PaintNode(view);
			conversation.AddNode(data);
		}
	}
}

// Footnotes:

// 2) This is necessary. If you validate a TextField by changing the focus on another element, this is
// not necessary but if you press ENTER to confirm, Unity will group the action with the last one

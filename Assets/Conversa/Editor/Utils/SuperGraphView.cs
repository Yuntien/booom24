using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class SuperGraphView : GraphView
	{
		protected UnityEvent<GraphViewChange> OnMoveElements { get; } = new UnityEvent<GraphViewChange>();
		protected UnityEvent<GraphViewChange> OnDeleteElements { get; } = new UnityEvent<GraphViewChange>();
		protected UnityEvent<GraphViewChange> OnNewElements { get; } = new UnityEvent<GraphViewChange>();
		protected UnityEvent<NodeCreationContext> OnNodeCreationRequest { get; } = new UnityEvent<NodeCreationContext>();

		protected UnityEvent<Group, IEnumerable<GraphElement>> OnElementsAddedToGroup { get; } =
			new UnityEvent<Group, IEnumerable<GraphElement>>();

		protected UnityEvent<Group, IEnumerable<GraphElement>> OnElementsRemovedFromGroup { get; } =
			new UnityEvent<Group, IEnumerable<GraphElement>>();

		protected UnityEvent<StackNode, int, IEnumerable<GraphElement>> OnElementsAddedToStack { get; } =
			new UnityEvent<StackNode, int, IEnumerable<GraphElement>>();

		protected UnityEvent<StackNode, IEnumerable<GraphElement>> OnElementsRemovedFromStack { get; } =
			new UnityEvent<StackNode, IEnumerable<GraphElement>>();
		

		protected SuperGraphView()
		{
			Setup();
			graphViewChanged = HandleGraphViewChange;
			elementsAddedToGroup = OnElementsAddedToGroup.Invoke;
			elementsRemovedFromGroup = OnElementsRemovedFromGroup.Invoke;
			nodeCreationRequest = OnNodeCreationRequest.Invoke;
			elementsInsertedToStackNode = OnElementsAddedToStack.Invoke;
			elementsRemovedFromStackNode = OnElementsRemovedFromStack.Invoke;
		}

		private void Setup()
		{
			SetupGrid();
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale * 2);

			this.StretchToParentSize();

			viewDataKey = "Do not delete me. I dare you, MoFo. I double dare you."; // Footnote #1

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}

		private GraphViewChange HandleGraphViewChange(GraphViewChange change)
		{
			OnMoveElements.Invoke(change);
			OnDeleteElements.Invoke(change);
			OnNewElements.Invoke(change);

			return change;
		}

		// The problem with this method is that it does not call "graphViewChanged"
		// Use "DeleteElement" instead
		[Obsolete("Do not use this method")]
		public new void RemoveElement(GraphElement element) => base.RemoveElement(element);

		protected void SetupGrid()
		{
			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();
		}
	}
}

// Footnotes:

// 1) If no "View Data Key" is set, Unity will reset many visual properties on every domain reload.
// To keep the "Zoom" and "Pan" of the GraphView persisted this needs to be set to a (unique?) key.
// ¯\_(ツ)_/¯ Don't ask me. It is like it is.
// https://docs.unity3d.com/Manual/UIE-ViewData.html
// https://forum.unity.com/threads/can-someone-explain-the-view-data-key-and-its-uses.855145/#post-5638936

using System.Linq;
using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class BookmarkJumpNodeView : BaseNodeView<BookmarkJumpNode>
	{
		protected override string Title => "Bookmark Jump";

		private VisualElement MainContainer => inputContainer.Q<Label>().parent;

		private Label MainLabel => this.Q<Label>(classes: "connectorText");

		private BookmarkNode Bookmark => Conversation.GetBookmark(Data.BookmarkGuid);

		// Constructor

		public BookmarkJumpNodeView(Conversation conversation) : base(new BookmarkJumpNode(), conversation)
		{
			schedule.Execute(UpdateName).Every(100);
		}

		public BookmarkJumpNodeView(BookmarkJumpNode data, Conversation conversation) : base(data, conversation)
		{
			schedule.Execute(UpdateName).Every(100);
		}
		

		// Methods

		protected override void SetPorts()
		{
			var port = new CustomPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(BaseNode));
			AddInputPort(port, Bookmark.Name, "previous");
		}

		protected override void SetBody() => MainContainer.Add(Icons.Arrow());

		private void UpdateName() => MainLabel.text = Bookmark.Name;

		
		
		[DeleteSelectionHandler]
		private static void HandleDeleteNodes(ConversaGraphView graphView)
		{
			var bookmarks = graphView.selection.OfType<BookmarkNodeView>().ToList();
			var jumps = graphView.GetNodes<BookmarkJumpNodeView>().ToList();
			
			bookmarks
				.Select(bookmark => jumps.Where(jump => jump.Bookmark.Guid == bookmark.Guid))
				.ToList()
				.ForEach(x => graphView.selection.AddRange(x));
		}
	}
}
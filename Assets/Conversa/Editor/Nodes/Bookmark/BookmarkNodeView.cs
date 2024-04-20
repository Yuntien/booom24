using UnityEngine.UIElements;
using Conversa.Runtime;
using Conversa.Runtime.Nodes;

namespace Conversa.Editor
{
	public class BookmarkNodeView : BaseNodeView<BookmarkNode>
	{
		protected override string Title => "Bookmark";

		private string BookmarkName => Data.Name;

		// Constructors

		public BookmarkNodeView(Conversation conversation) : this(new BookmarkNode(), conversation)
		{
			schedule.Execute(UpdateName).Every(100);
		}

		
		public BookmarkNodeView(BookmarkNode data, Conversation conversation) : base(data, conversation)
		{
			schedule.Execute(UpdateName).Every(100);
		}

		// Methods

		protected override void SetBody()
		{
			this.Q<Label>(classes: "connectorText").parent.Add(Icons.Bookmark());
		}
		
		private void UpdateName() => this.Q<Label>(classes: "connectorText").text = BookmarkName;
	}
}
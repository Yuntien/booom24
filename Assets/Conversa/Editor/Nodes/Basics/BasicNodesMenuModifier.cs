using System.Linq;
using Conversa.Editor.unity_conversa.Editor.Nodes.LIterals;
using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using Conversa.Runtime.Properties;
using unity_conversa.Editor.Nodes;

namespace Conversa.Editor
{
	public class BasicNodesMenuModifier
	{
		[NodeMenuModifier]
		private static void ModifyMenu(NodeMenuTree tree, Conversation conversation)
		{
			tree.AddGroup("Basics");
			tree.AddMenuEntry<MessageNodeView>("Message", 2);
			tree.AddMenuEntry<AdvancedMessageNodeView>("Advanced Message", 2);
			tree.AddMenuEntry<ChoiceNodeView>("Choice", 2);
			tree.AddMenuEntry<ConditionalNodeView>("Branch", 2);
			tree.AddMenuEntry<BookmarkNodeView>("Bookmark", 2);
			tree.AddMenuEntry<RandomFlowNodeView>("Random", 2);

			tree.AddGroup("Jumps", 2);

			conversation.AllNodes.OfType<BookmarkNode>().ToList().ForEach(bookmark =>
			{
				BookmarkJumpNodeView Callback()
				{
					var node = new BookmarkJumpNode { BookmarkName = bookmark.Name, BookmarkGuid = bookmark.Guid};
					var view = new BookmarkJumpNodeView(node, conversation);
					return view;
				}

				tree.AddMenuEntry("Jump: " + bookmark.Name, Callback, 3);
			});

			tree.AddGroup("Literals");
			tree.AddMenuEntry<LiteralStringNodeView>("String", 2);
			tree.AddMenuEntry<AbsoluteFloatNodeView>("Float", 2);
			tree.AddMenuEntry<AbsoluteBoolNodeView>("Boolean", 2);
			tree.AddMenuEntry<ActorNodeView>("Actor", 2);

			tree.AddGroup("Text tools");
			tree.AddMenuEntry<ParseNodeView>("Parse", 2);
			tree.AddMenuEntry<FloatToStringNodeView>("ToString", 2);

			tree.AddGroup("Events");

			conversation.Properties.OfType<EventProperty>().ToList().ForEach(property =>
			{
				EventNodeView Callback()
				{
					var node = new EventNode(property.Guid, property.Name, ConversaSettings.instance.StopEventsByDefault);
					var view = new EventNodeView(node, conversation);
					return view;
				}

				tree.AddMenuEntry(property.Name, Callback, 2);
			});
		}
	}
}
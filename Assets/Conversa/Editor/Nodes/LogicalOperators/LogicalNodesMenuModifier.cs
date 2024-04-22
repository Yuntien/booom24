using Conversa.Runtime;

namespace Conversa.Editor.Nodes.LogicalOperators
{
	public class LogicalNodesMenuModifier
	{
		[NodeMenuModifier(3)]
		private static void ModifyMenu(NodeMenuTree tree, Conversation conversation)
		{
			tree.AddGroup("Logical operators");
			tree.AddMenuEntry<AndNodeView>("And", 2);
			tree.AddMenuEntry<OrNodeView>("Or", 2);
			tree.AddMenuEntry<NorNodeView>("Nor", 2);
			tree.AddMenuEntry<NorNodeView>("Xor", 2);
			tree.AddMenuEntry<NotNodeView>("Not", 2);
		}
	}
}
using Conversa.Editor.Nodes.Math;
using Conversa.Runtime;

namespace Conversa.Editor
{
	public class MathNodesMenuModifier
	{
		[NodeMenuModifier(3)]
		private static void ModifyMenu(NodeMenuTree tree, Conversation conversation)
		{
			tree.AddGroup("Math operators");

			tree.AddMenuEntry<AddNodeView>("Add", 2);
			tree.AddMenuEntry<SubtractNodeView>("Subtract", 2);
			tree.AddMenuEntry<MultiplyNodeView>("Multiply", 2);
			tree.AddMenuEntry<DivideNodeView>("Divide", 2);

			tree.AddMenuEntry<CompareNodeView>("Compare number", 2);
			tree.AddMenuEntry<RandomFloatView>("Random", 2);
			tree.AddGroup("Obsolete", 2);
			tree.AddMenuEntry<GreaterThanNodeView>("Greater than", 3);
			tree.AddMenuEntry<LessThanNodeView>("Less than", 3);
			tree.AddMenuEntry<EqualsNodeView>("Equals", 3);
		}
	}
}


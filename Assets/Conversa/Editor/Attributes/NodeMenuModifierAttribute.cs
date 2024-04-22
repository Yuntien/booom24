using System;

namespace Conversa.Editor
{
	// This attribute is used to add new entries into the "Add node" menu
	public class NodeMenuModifierAttribute : Attribute
	{
		public int Position { get; }

		public NodeMenuModifierAttribute(int position = 1)
		{
			Position = position;
		}
	}
}
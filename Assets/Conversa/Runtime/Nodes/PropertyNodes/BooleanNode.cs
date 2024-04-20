using System;
using Conversa.Runtime.Nodes.PropertyNodes;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes
{
	// Deprecated class. Use PropertyNode instead.
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]		
	[Port("Next", "next", typeof(bool), Flow.Out, Capacity.Many)]
	public class BooleanNode : PropertyNode
	{
		public BooleanNode(string guid, string name) : base(guid, name) { }
	}
}
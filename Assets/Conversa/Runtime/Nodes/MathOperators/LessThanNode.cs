using System;
using Conversa.Runtime.Interfaces;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Nodes.MathOperators
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[Port("A", "in-a", typeof(float), Flow.In, Capacity.One)]
	[Port("B", "in-b", typeof(float), Flow.In, Capacity.One)]
	[Port("Out", "out", typeof(bool), Flow.Out, Capacity.Many)]
	public class LessThanNode : BaseNode, IValueNode
	{
		public T GetValue<T>(string portGuid, Conversation conversation)
		{
			if (portGuid != "out") return default;

			var value1 = conversation.GetConnectedValueTo<float>(this, "in-a");
			var value2 = conversation.GetConnectedValueTo<float>(this, "in-b");

			var output = value1 < value2;

			return Converter.ConvertValue<bool, T>(output);
		}
	}
}
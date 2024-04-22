using System;
using Conversa.Runtime.Attributes;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Properties
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[ConversationProperty("Float", 48, 63, 159)]
	public class FloatProperty : ValueProperty<float>
	{
		public FloatProperty(string name) : base(name) { }
	}
}
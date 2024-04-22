using System;
using Conversa.Runtime.Attributes;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Properties
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[ConversationProperty("Boolean", 0.18f, 0.49f, 0.20f)]
	public class BooleanProperty : ValueProperty<bool>
	{
		public BooleanProperty(string name) : base(name) { }
	}
}

#region Deprecated

namespace Conversa.Runtime
{
	public static class ConversationBoolExtensions
	{
		[Obsolete("Please, use 'conversation.SetProperty<bool>()' instead of 'SetBool'")]
		public static void SetBool(this Conversation conversation, string name, bool value) =>
			conversation.SetProperty<bool>(name, value);

		[Obsolete("Please, use 'conversation.GetProperty<bool>()' instead of 'GetBool'")]
		public static bool GetBool(this Conversation conversation, string name) =>
			conversation.GetProperty<bool>(name);

		[Obsolete("Please, use 'conversation.SetProperty<bool>()' instead of 'SetBooleanValue'")]
		public static void SetBooleanValue(this Conversation conversation, string propertyName, bool value) =>
			conversation.SetBool(propertyName, value);

	}
}

#endregion
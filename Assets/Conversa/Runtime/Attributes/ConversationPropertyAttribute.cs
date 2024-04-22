using System;
using UnityEngine;

namespace Conversa.Runtime.Attributes
{
	public class ConversationPropertyAttribute : Attribute
	{
		public string PropertyName { get; }
		public Color Color { get; }

		public ConversationPropertyAttribute(string propertyName, float r, float g, float b)
		{
			PropertyName = propertyName;
			Color = new Color(r, g, b);
		}

		public ConversationPropertyAttribute(string propertyName, int r, int g, int b) :
			this(propertyName, r / 255f, g / 255f, b / 255f) { }

	}
}
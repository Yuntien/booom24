using System;
using Conversa.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class CustomPort : Port
	{
		private static readonly Color FlowColor = GetColor(255, 255, 255);

		private static readonly Color StringColor = GetColor(130, 211, 246);
		
		private static readonly Color BoolColor = new Color(0.51f, 0.85f, 0.53f);

		private static readonly Color FloatColor = GetColor(101, 114, 183);

		private static readonly Color ActorColor = GetColor(181, 104, 183);

		public CustomPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) :
			base(portOrientation, portDirection, portCapacity, type)
		{
			m_EdgeConnector = new EdgeConnector<Edge>(new FooConnector());

			this.AddManipulator(m_EdgeConnector);

			if (type == typeof(BaseNode))
				portColor = FlowColor;
			else if (type == typeof(bool))
				portColor = BoolColor;
			else if (type == typeof(float))
				portColor = FloatColor;
			else if (type == typeof(string))
				portColor = StringColor;
			else if (type == typeof(Actor))
				portColor = ActorColor;
		}
		
		static Color GetColor(int r, int g, int b) => new Color(r/255f, g/255f, b/255f);
	}
}
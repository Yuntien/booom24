using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	public class NodePort : IEquatable<NodePort>
	{
		[SerializeField] private string node;
		[SerializeField] private string port;

		public string Node { get => node; set => node = value; }
		public string Port { get => port; set => port = value; }

		public NodePort(string node, string port)
		{
			this.node = node;
			this.port = port;
		}

		public bool IsValid() => !string.IsNullOrEmpty(node) && !string.IsNullOrEmpty(port);

		// IEquatable

		public bool Equals(NodePort other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return node == other.node && port == other.port;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((NodePort)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((node != null ? node.GetHashCode() : 0) * 397) ^ (port != null ? port.GetHashCode() : 0);
			}
		}
	}

}
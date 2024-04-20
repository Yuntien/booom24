using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	public class EdgeData : IEquatable<EdgeData>
	{
		[SerializeField] private NodePort output;
		[SerializeField] private NodePort input;

		public NodePort Output => output;
		public NodePort Input => input;

		public EdgeData(NodePort output, NodePort input)
		{
			this.output = output;
			this.input = input;
		}

		public bool IsValid() => output.IsValid() && input.IsValid();

		public bool Contains(NodePort end) => output.Equals(end) || input.Equals(end);

		public NodePort Opposite(NodePort end) => end.Equals(output) ? input : output;

		public override string ToString() => $"OUT: ({output.Node},{output.Port}) IN: ({input.Node},{input.Port})";

		// IEquatable

		public bool Equals(EdgeData other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return output.Equals(other.output) && input.Equals(other.input);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((EdgeData)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((output != null ? output.GetHashCode() : 0) * 397) ^ (input != null ? input.GetHashCode() : 0);
			}
		}
	}
}
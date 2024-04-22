using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	public class PortDefinition<T>
	{
		[SerializeField] private string guid;
		[SerializeField] private string label;
		
		public string Guid { get => guid; set => guid = value; }
		public string Label { get => label; set => label = value; }

		public PortDefinition(string guid, string label)
		{
			this.guid = guid;
			this.label = label;
		}
	}
}
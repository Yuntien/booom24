using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime.Properties
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	public class BaseProperty
	{
		[field: SerializeField] public string Guid { get; private set; }
		[SerializeField] private string name;

		public string Name
		{
			get => name;
			set => name = value;
		}

		public BaseProperty(string name)
		{
			Guid = System.Guid.NewGuid().ToString();
			this.name = name;
		}
	}
}
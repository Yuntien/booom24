using System;
using UnityEngine;
using Conversa.Runtime.Interfaces;
using UnityEngine.Scripting.APIUpdating;
using Random = UnityEngine.Random;

namespace Conversa.Runtime
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	[Port("Out", "out", typeof(bool), Flow.Out, Capacity.Many)]
	public class RandomFloatNode : BaseNode, IValueNode
	{
		[SerializeField] private float min = 0;
		[SerializeField] private float max = 1;

		public float Min
		{
			get => min;
			set => min = value;
		}

		public float Max
		{
			get => max;
			set => max = value;
		}

		public T GetValue<T>(string portGuid, Conversation conversation)
		{
			if (portGuid != "out") return default;

			var output = Random.Range(min, max);

			return Converter.ConvertValue<float, T>(output);
		}
	}
}
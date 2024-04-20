using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Conversa.Runtime.Interfaces;
using UnityEngine.Scripting.APIUpdating;

namespace Conversa.Runtime
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	public class BaseNode : INode
	{
		[field: SerializeField] public string Guid { get; set; }
		[field: SerializeField] public Rect NodeRect { get; set; }
		
		protected BaseNode()
		{
			Guid = System.Guid.NewGuid().ToString();
		}

		public virtual bool IsValid(Conversation conversation) => true;

		public NodePort GetNodePort(string portGuid) => new NodePort(Guid, portGuid);

		public IEnumerable<PortAttribute> GetPortAttributes() => GetType().GetCustomAttributes<PortAttribute>();

		public virtual bool ContainsPort(string portId, Flow flow) =>
			ContainsPortAttribute(portId, flow) || ContainsSlot(portId, flow);

		public IEnumerable<PropertyInfo> GetSlotProperties() =>
			GetType().GetProperties().Where(property => property.GetCustomAttribute<SlotAttribute>() != null);

		private bool ContainsSlot(string portId, Flow flow) =>
			GetSlotProperties()
				.Select(property => property.GetCustomAttribute<SlotAttribute>())
				.Any(attr => attr.Id == portId && attr.Flow == flow);

		public virtual bool ContainsPortAttribute(string portId, Flow flow)
		{
			var attributes = GetPortAttributes();
			foreach (var attribute in attributes)
			{
				if (attribute.Instances == 1 && attribute.Id == portId && attribute.Flow == flow)
					return true;
				if (attribute.Instances > 1)
					for (var i = 1; i < attribute.Instances; i++)
						if ($"{attribute.Id} {i}" == portId && attribute.Flow == flow)
							return true;
			}

			return false;
		}

		protected T ProcessPort<T>(Conversation conversation, string portGuid, T fallback) =>
			conversation != null && conversation.IsConnected(Guid, portGuid)
				? conversation.GetConnectedValueTo<T>(this, portGuid)
				: fallback;
	}
}
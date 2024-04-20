using System;

namespace Conversa.Runtime
{
	public enum Flow { In, Out }
	public enum Capacity { One, Many }

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class PortAttribute : Attribute
	{
		public string Name { get; }
		public string Id { get; }
		public Type Type { get; }
		public Flow Flow { get; }
		public Capacity Capacity { get; }
		public int Instances { get; }

		public PortAttribute(string name, string id, Type type, Flow flow, Capacity capacity, int instances = 1)
		{
			Name = name;
			Id = id;
			Type = type;
			Flow = flow;
			Capacity = capacity;
			Instances = instances;
		}
	}
	
	[AttributeUsage(AttributeTargets.Property)]
	public class SlotAttribute : Attribute
	{
		public string Name { get; }
		public string Id { get; }
		public Flow Flow { get; }
		public Capacity Capacity { get; }

		public SlotAttribute(string name, string id, Flow flow, Capacity capacity)
		{
			Name = name;
			Id = id;
			Flow = flow;
			Capacity = capacity;
		}
	}
}


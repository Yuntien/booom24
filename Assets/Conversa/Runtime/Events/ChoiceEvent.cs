using System;
using System.Collections.Generic;
using Conversa.Runtime.Interfaces;

namespace Conversa.Runtime.Events
{
	public class Option
	{
		public string Message { get; }
		public Action Advance { get; }

		public Option(string message, Action advance)
		{
			Message = message;
			Advance = advance;
		}
	}

	public class ChoiceEvent : IConversationEvent
	{
		public string Actor { get; }
		public string Message { get; }
		public List<Option> Options { get; }

		public ChoiceEvent(string actor, string message, List<Option> options)
		{
			Actor = actor;
			Message = message;
			Options = options;
		}
	}
}
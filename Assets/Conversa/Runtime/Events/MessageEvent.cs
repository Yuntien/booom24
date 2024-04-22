using System;
using Conversa.Runtime.Interfaces;

namespace Conversa.Runtime.Events
{
	public class MessageEvent : IConversationEvent
	{
		public string Actor { get; }
		public string Message { get; }
		public Action Advance { get; }

		public MessageEvent(string actor, string message, Action advance)
		{
			Actor = actor;
			Message = message;
			Advance = advance;
		}
	}
}
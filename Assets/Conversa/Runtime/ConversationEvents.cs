using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using UnityEngine.Events;

namespace Conversa.Runtime
{
	public class ConversationEvents
	{
		#region deprecated

		public UnityEvent<MessageEvent> OnMessage { get; } = new UnityEvent<MessageEvent>();
		public UnityEvent<ChoiceEvent> OnChoice { get; } = new UnityEvent<ChoiceEvent>();
		public UnityEvent<UserEvent> OnUserEvent { get; } = new UnityEvent<UserEvent>();
		public UnityEvent OnEnd { get; } = new UnityEvent();

		#endregion

		public UnityEvent<IConversationEvent> OnConversationEvent { get; } = new UnityEvent<IConversationEvent>();
	}
}
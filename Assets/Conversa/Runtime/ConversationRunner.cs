using System;
using System.Collections.Generic;
using System.Linq;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using Conversa.Runtime.Properties;
using UnityEngine.Events;

namespace Conversa.Runtime
{
	public class ConversationRunner
	{
		private readonly Conversation conversation;
		private readonly Conversation clone;
		private readonly ConversationEvents conversationEvents = new ConversationEvents();
		private bool isRunning;

		public bool IsRunning => isRunning;
		private string currentNodeGuid = String.Empty;

		public string CurrentNodeGuid => currentNodeGuid;

		public UnityEvent<IConversationEvent> OnConversationEvent => conversationEvents.OnConversationEvent;

		#region deprecated

		public UnityEvent<MessageEvent> OnMessage => conversationEvents.OnMessage;
		public UnityEvent<ChoiceEvent> OnChoice => conversationEvents.OnChoice;
		public UnityEvent<UserEvent> OnUserEvent => conversationEvents.OnUserEvent;
		public UnityEvent OnEnd => conversationEvents.OnEnd;

		#endregion

		public ConversationRunner(Conversation conversation)
		{
			this.conversation = conversation;
			clone = conversation.Clone();
			ResetProperties();
			clone.OnEventNodeProcessed.AddListener(node => currentNodeGuid = node.Guid);
		}

		public void ResetProperties() => clone.GetValueProperties().ToList().ForEach(x => x.Reset());

		// Start running
		public void Begin()
		{
			BeginRun(clone.StartNode);
		}

		public void BeginByGuid(string nodeGuid)
		{
			var startPoint = clone.AllNodes.FirstOrDefault(node => node.Guid == nodeGuid);
			if (startPoint is IEventNode eventNode)
				BeginRun(eventNode);
			else
				Begin();
		}

		private void BeginRun(IEventNode node)
		{
			isRunning = true;
			node.Process(clone, conversationEvents);
		}

		// Start from a certain bookmark
		public void Begin(string bookmark) => BeginRun(clone.GetBookmarkNode(bookmark));

		// Used by the user to update properties
		public void SetProperty<T>(string name, T value) => clone.SetProperty(name, value);

		public List<BaseProperty> GetProperties() => clone.Properties;

		public T GetProperty<T>(string name) => clone.GetProperty<T>(name);
	}
}
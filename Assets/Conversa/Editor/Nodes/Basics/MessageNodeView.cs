using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
	public class MessageNodeView : BaseNodeView<MessageNode>
	{
		protected override string Title => "Message";

		// Constructors

		public MessageNodeView(Conversation conversation)
			: base(new MessageNode(ConversaSettings.instance.UseActorProfileByDefault), conversation) { }

		public MessageNodeView(MessageNode data, Conversation conversation) : base(data, conversation) { }

		private Label actorLabel;
		private Label messageLabel;

		// Methods
		
		protected override void SetBody()
		{
			var template = Resources.Load<VisualTreeAsset>("NodeViews/MessageNode");
			template.CloneTree(bodyContainer);
			
			actorLabel = bodyContainer.Q<Label>("actor");
			messageLabel = bodyContainer.Q<Label>("message");
			UpdateValues();

			schedule.Execute(UpdateValues).Every(100);
		}

		private void UpdateValues()
		{
			if (actorLabel.text != Data.ActorName)
				actorLabel.text = Data.ActorName;

			if (Data.Message != messageLabel.text)
				messageLabel.text = Data.Message;
		}
	}
}
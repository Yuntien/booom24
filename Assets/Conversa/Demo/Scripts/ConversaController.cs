using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Conversa.Demo.Scripts
{
	public class ConversaController : MonoBehaviour
	{
		[SerializeField] private Conversation conversation;
		[SerializeField] private UIController uiController;

		[Header("Buttons")]
		[SerializeField] private Button restartConversationButton;
		[SerializeField] private Button updateSavepointButton;
		[SerializeField] private Button loadSavepointButton;

		private ConversationRunner runner;
		private string savepointGuid = string.Empty;

		private void Start()
		{
			runner = new ConversationRunner(conversation);
			runner.OnConversationEvent.AddListener(HandleConversationEvent);
			restartConversationButton.onClick.AddListener(HandleRestartConversation);
			updateSavepointButton.onClick.AddListener(HandleUpdateSavepoint);
			loadSavepointButton.onClick.AddListener(HandleLoadSavepoint);
			updateSavepointButton.interactable = false;
		}

		private void HandleConversationEvent(IConversationEvent e)
		{
			switch (e)
			{
				case MessageEvent messageEvent:
					HandleMessage(messageEvent);
					break;
				case ChoiceEvent choiceEvent:
					HandleChoice(choiceEvent);
					break;
				case ActorMessageEvent actorMessageEvent:
					HandleActorMessageEvent(actorMessageEvent);
					break;
				case ActorChoiceEvent actorChoiceEvent:
					HandleActorChoiceEvent(actorChoiceEvent);
					break;
				case UserEvent userEvent:
					HandleUserEvent(userEvent);
					break;
				case EndEvent _:
					HandleEnd();
					break;
			}
		}

		private void HandleActorMessageEvent(ActorMessageEvent evt)
		{
			
			var actorDisplayName = evt.Actor == null ? "" : evt.Actor.DisplayName;
			if (evt.Actor is AvatarActor avatarActor)
				uiController.ShowMessage(actorDisplayName, evt.Message, avatarActor.Avatar, evt.Advance);
			else
				uiController.ShowMessage(actorDisplayName, evt.Message, null, evt.Advance);
		}

		private void HandleActorChoiceEvent(ActorChoiceEvent evt)
		{
			var actorDisplayName = evt.Actor == null ? "" : evt.Actor.DisplayName;
			if (evt.Actor is AvatarActor avatarActor)
				uiController.ShowChoice(actorDisplayName, evt.Message, avatarActor.Avatar, evt.Options);
			else
				uiController.ShowChoice(actorDisplayName, evt.Message, null, evt.Options);
		}

		private void HandleMessage(MessageEvent e) => uiController.ShowMessage(e.Actor, e.Message, null, () => e.Advance());

		private void HandleChoice(ChoiceEvent e) => uiController.ShowChoice(e.Actor, e.Message, null, e.Options);

		private static void HandleUserEvent(UserEvent userEvent)
		{
			if (userEvent.Name == "Food bought")
				Debug.Log("We can use this event to update the inventory, for instance");
		}

		private void HandleRestartConversation()
		{
			runner.Begin();
			updateSavepointButton.interactable = true;
		}

		private void HandleLoadSavepoint()
		{
			runner.BeginByGuid(savepointGuid);
		}

		private void HandleUpdateSavepoint()
		{
			savepointGuid = runner.CurrentNodeGuid;
		}

		private void HandleEnd()
		{
			uiController.Hide();
			updateSavepointButton.interactable = false;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using System;

public class ConversationController : Singleton<ConversationController>
{
    private ConversationRunner runner;

    private Action tempAction;

    /// <summary>
    /// �����Ի�
    /// </summary>
    /// <param name="conversation">�Ի�</param>
    public void StartConversation(Conversation conversation)
    {
        runner = new ConversationRunner(conversation);
        runner.OnConversationEvent.AddListener(HandleConversationEvent);

        // �����Ի�
        runner.Begin();
    }

    /// <summary>
    /// �����Ի�
    /// </summary>
    public void ContinueConversation()
    {
        tempAction.Invoke();
    }

    private void HandleConversationEvent(IConversationEvent evt)
    {
        Debug.Log(evt.ToString());
        switch (evt)
        {
            case MessageEvent messageEvent:
                Debug.Log("MessageEvent");
                break;
            case ChoiceEvent choiceEvent:
                Debug.Log("ChoiceEvent");
                break;
            case ActorMessageEvent actorMessageEvent:
                // LinearDialogaue�ڵ�����actor��Message
                HandleActorMessageEvent(actorMessageEvent);
                break;
            case ActorChoiceEvent actorChoiceEvent:
                Debug.Log("actChoiceEvent");
                break;
            case UserEvent userEvent:
                HandleUserEvent(userEvent);
                break;
            case EndEvent endEvent:
                HandleEnd();
                break;
            default:
                break;
        }
    }

    private void HandleUserEvent(UserEvent userEvent)
    {
        if (userEvent.Name == "��ʼά��")
        {
            DialogUIController.Instance.Hide();
            tempAction = userEvent.Advance;
            Debug.Log(userEvent.Name);
            GameManager.Instance.Fix();
        }
    }

    private void HandleActorMessageEvent(ActorMessageEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        DialogUIController.Instance.ShowMessage(actorName, evt.Message, evt.Advance);
    }

    private void HandleEnd()
    {
        Debug.Log("�Ի���ɣ����ضԻ���");
        DialogUIController.Instance.Hide();
    }
}



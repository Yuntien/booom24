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

    private Dictionary<string, Option> optionMap = new Dictionary<string, Option>();

    [Header("�����ڸǹ㲥")]
    [SerializeField] private FadeEventSO sceneFadeEventSO;
    [SerializeField] private FadeEventSO fadeEventSO;

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

    /// <summary>
    /// ����ѡ��
    /// </summary>
    /// <param name="choiceName">ѡ������</param>
    public void ContinueChoice(string choiceName)
    {
        if (optionMap.ContainsKey(choiceName))
        {
            var choice = optionMap[choiceName];
            choice.Advance.Invoke();
        }
    }

    private void HandleConversationEvent(IConversationEvent evt)
    {
        switch (evt)
        {
            case MessageEvent messageEvent:
                HandleMessageEvent(messageEvent);
                break;
            case ChoiceEvent choiceEvent:
                // ����¼�ר�Ź�����ʹ��
                HandleChoiceEvent(choiceEvent);
                break;
            case ActorMessageEvent actorMessageEvent:
                // LinearDialogaue�ڵ�����actor��Message
                HandleActorMessageEvent(actorMessageEvent);
                break;
            case ActorChoiceEvent actorChoiceEvent:
                HandleActorChoiceEvent(actorChoiceEvent);
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

    private void HandleMessageEvent(MessageEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor;
        DialogUIController.Instance.ShowMessage(actorName, evt.Message, evt.Advance);
    }

    private void HandleChoiceEvent(ChoiceEvent evt)
    {
        DialogUIController.Instance.Hide();
        optionMap.Clear();
        // ��������¼��Ͱ�ѡ���ݴ�����
        evt.Options.ForEach(option =>
        {
            optionMap.Add(option.Message, option);
        });
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
        else if (userEvent.Name == "���볡��")
        {
            // sceneFadeEventSO.FadeOut(0.5f);
            Menu.Instance.FadeOut(1f).onComplete += () =>
            {
                userEvent.Advance.Invoke();
            };
        }
        else if (userEvent.Name == "���в��")
        {
            DialogUIController.Instance.Hide();
            // TODO �򿪽��в�����
            UIManager.instance.ShowRemoveWindow();
            tempAction = userEvent.Advance;
        }
        else if (userEvent.Name == "ά�����")
        {
            DialogUIController.Instance.Hide();
            // ά�����
            tempAction = userEvent.Advance;
            GameManager.Instance.ContinueTalk();
        }
        else if (userEvent.Name == "���ﵭ��")
        {
            GuestController.Instance.GuestFadeIn();
        }
        else if (userEvent.Name == "���ﵭ��")
        {
            GuestController.Instance.GuestFadeOut();
        }
        else if (userEvent.Name == "�������")
        {
            GuestController.Instance.ShowGuest();
        }
        else if (userEvent.Name == "��������")
        {
            GuestController.Instance.HideGuest();
        }
        else if (userEvent.Name == "�����ƶ���ά�޴�")
        {
            tempAction = userEvent.Advance;
            GuestController.Instance.MoveGuestToFix().onComplete += () => {
                tempAction.Invoke();
            };
        }
        else if (userEvent.Name == "���ֵ���")
        {
            FadeCanvas.Instance.FadeIn(1f).onComplete += () =>
            {
                userEvent.Advance.Invoke();
            };
        }
    }
    private void HandleActorMessageEvent(ActorMessageEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        DialogUIController.Instance.ShowMessage(actorName, evt.Message, evt.Advance);
    }

    private void HandleActorChoiceEvent(ActorChoiceEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        DialogUIController.Instance.ShowChoice(actorName, evt.Message, evt.Options);
    }

    private void HandleEnd()
    {
        Debug.Log("�Ի���ɣ����ضԻ���");
        DialogUIController.Instance.Hide();
        GameManager.Instance.NextTalk();
    }
}



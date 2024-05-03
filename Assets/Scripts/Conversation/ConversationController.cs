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

    [Header("场景遮盖广播")]
    [SerializeField] private FadeEventSO sceneFadeEventSO;
    [SerializeField] private FadeEventSO fadeEventSO;

    /// <summary>
    /// 开启对话
    /// </summary>
    /// <param name="conversation">对话</param>
    public void StartConversation(Conversation conversation)
    {
        runner = new ConversationRunner(conversation);
        runner.OnConversationEvent.AddListener(HandleConversationEvent);

        // 开启对话
        runner.Begin();
    }

    /// <summary>
    /// 继续对话
    /// </summary>
    public void ContinueConversation()
    {
        tempAction.Invoke();
    }

    /// <summary>
    /// 继续选项
    /// </summary>
    /// <param name="choiceName">选项名称</param>
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
                // 这个事件专门供接线使用
                HandleChoiceEvent(choiceEvent);
                break;
            case ActorMessageEvent actorMessageEvent:
                // LinearDialogaue节点混合了actor和Message
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
        // 进入接线事件就把选项暂存起来
        evt.Options.ForEach(option =>
        {
            optionMap.Add(option.Message, option);
        });
    }

    private void HandleUserEvent(UserEvent userEvent)
    {
        if (userEvent.Name == "开始维修")
        {
            DialogUIController.Instance.Hide();
            tempAction = userEvent.Advance;
            Debug.Log(userEvent.Name);
            GameManager.Instance.Fix();
        } 
        else if (userEvent.Name == "进入场景")
        {
            // sceneFadeEventSO.FadeOut(0.5f);
            Menu.Instance.FadeOut(1f).onComplete += () =>
            {
                userEvent.Advance.Invoke();
            };
        }
        else if (userEvent.Name == "进行拆除")
        {
            DialogUIController.Instance.Hide();
            // TODO 打开进行拆除面板
            UIManager.instance.ShowRemoveWindow();
            tempAction = userEvent.Advance;
        }
        else if (userEvent.Name == "维修完成")
        {
            DialogUIController.Instance.Hide();
            // 维修完成
            tempAction = userEvent.Advance;
            GameManager.Instance.ContinueTalk();
        }
        else if (userEvent.Name == "人物淡入")
        {
            GuestController.Instance.GuestFadeIn();
        }
        else if (userEvent.Name == "人物淡出")
        {
            GuestController.Instance.GuestFadeOut();
        }
        else if (userEvent.Name == "人物出现")
        {
            GuestController.Instance.ShowGuest();
        }
        else if (userEvent.Name == "人物隐藏")
        {
            GuestController.Instance.HideGuest();
        }
        else if (userEvent.Name == "人物移动至维修处")
        {
            tempAction = userEvent.Advance;
            GuestController.Instance.MoveGuestToFix().onComplete += () => {
                tempAction.Invoke();
            };
        }
        else if (userEvent.Name == "遮罩淡入")
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
        Debug.Log("对话完成，隐藏对话框");
        DialogUIController.Instance.Hide();
        GameManager.Instance.NextTalk();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using System;
using DG.Tweening;

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
        Debug.Log(choiceName);
        if (optionMap.ContainsKey(choiceName))
        {
            var choice = optionMap[choiceName];
            choice.Advance.Invoke();
        }
    }

    public void AllFound()
    {
        runner.SetProperty("全部找到", true);
    }

    public void SetPropertuy(string name, bool value)
    {
        runner.SetProperty(name, value);
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
            case GameControlEvent gameControlEvent:
                HandleGameControlEvent(gameControlEvent);
                break;
            case EndEvent endEvent:
                HandleEnd();
                break;
            default:
                break;
        }
    }

    private void HandleGameControlEvent(GameControlEvent evt)
    {
        switch (evt.Key)
        {
            case "延时":
                DialogUIController.Instance.Hide();
                float delayTime = float.Parse(evt.Value);
                StartCoroutine(Delay(delayTime, evt.Advance));
                break;
            case "播放记忆音效":
                AudioManager.Instance.PlayMemoryAudio(evt.Value);
                evt.Advance.Invoke();
                break;
            case "暂停记忆音效":
                AudioManager.Instance.PauseMemoryAudio();
                evt.Advance.Invoke();
                break;
            case "继续记忆音效":
                AudioManager.Instance.ContinueMemoryAudio();
                evt.Advance.Invoke();
                break;
            case "播放循环音效":
                AudioManager.Instance.PlayLoopAudio(evt.Value);
                evt.Advance.Invoke();
                break;
            case "暂停循环音效":
                AudioManager.Instance.PauseLoopAudio();
                evt.Advance.Invoke();
                break;
            case "继续循环音效":
                AudioManager.Instance.ContinueLoopAudio();
                evt.Advance.Invoke();
                break;
            case "播放交互音效":
                AudioManager.Instance.RandomPlayInteraction(evt.Value);
                evt.Advance.Invoke();
                break;
            case "切换人物图片":
                GuestController.Instance.ChangeGuestPic(evt.Actor.name, evt.Value);
                evt.Advance.Invoke();
                break;
            case "人物淡入":
                if (evt.Actor is DialogActor dialogActor)
                {
                    GuestController.Instance.GuestFadeIn(dialogActor.FileName, evt.Value).onComplete += () => evt.Advance.Invoke();
                }
                else
                {
                    GuestController.Instance.GuestFadeIn().onComplete += () => evt.Advance.Invoke();
                }
                break;
            case "人物淡出":
                GuestController.Instance.GuestFadeOut().onComplete += () => evt.Advance.Invoke();
                break;
            case "切换场景":
                DialogUIController.Instance.Hide();
                GameManager.Instance.SceneChange(evt.Value);
                tempAction = evt.Advance;
                break;
            case "展示图片":
                DialogUIController.Instance.Hide();
                Debug.Log("图片展示占位");
                evt.Advance.Invoke();
                break;
            case "电视切换":
                Tweener tweener = Menu.Instance.TVSwitch(evt.Value);
                if (tweener != null)
                {
                    tweener.onComplete += () => evt.Advance.Invoke();
                }
                else
                {
                    evt.Advance.Invoke();
                }
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
            Debug.Log("进行拆除");
            DialogUIController.Instance.Hide();
            // TODO 打开进行拆除面板
            DisassemblyManager.Instance.StartRepairMode(false);
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
        else if (userEvent.Name == "遮罩淡出")
        {
            FadeCanvas.Instance.FadeOut(1f).onComplete += () =>
            {
                userEvent.Advance.Invoke();
            };
        }
        else if (userEvent.Name == "进行深度维修")
        {
            tempAction = userEvent.Advance;
            GameManager.Instance.DeepFix();
        }
        else if (userEvent.Name == "深度维修拆除")
        {
            DialogUIController.Instance.Hide();
            tempAction = userEvent.Advance;
            DisassemblyManager.Instance.StartRepairMode(true);
        }
        else if (userEvent.Name == "深度维修完成")
        {
            // FIX 先销毁Maze
            //GameObject maze = GameObject.Find("Maze");
            //if (maze != null)
            //{
            //    Destroy(maze);
            //}
            MazeGenerator.Instance.DeleteMaze();

            tempAction = userEvent.Advance;
            DialogUIController.Instance.Hide();
            GameManager.Instance.ContinueTalk();
        }
        else if (userEvent.Name == "靠近窗口")
        {
            BackgroundController.Instance.MoveToWindow().OnComplete(() =>
            {
                userEvent.Advance.Invoke();
            });
        }
    }
    private void HandleActorMessageEvent(ActorMessageEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        DialogUIController.Instance.ShowMessage(actorName, evt.Message, evt.Advance);

        if (evt.Actor is DialogActor dialogActor)
        {
            // 对话时播放语音
            AudioManager.Instance.RandomPlayVoice(dialogActor.FileName, dialogActor.AudioNum);
        }
    }

    private void HandleActorChoiceEvent(ActorChoiceEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        DialogUIController.Instance.ShowChoice(actorName, evt.Message, evt.Options);

        if (evt.Actor is DialogActor dialogActor)
        {
            // 对话时播放语音
            AudioManager.Instance.RandomPlayVoice(dialogActor.FileName, dialogActor.AudioNum);
        }
    }

    private void HandleEnd()
    {
        Debug.Log("对话完成，隐藏对话框");
        DialogUIController.Instance.Hide();
        GameManager.Instance.NextTalk();
    }

    private IEnumerator Delay(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }
}



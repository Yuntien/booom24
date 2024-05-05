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
        Debug.Log(choiceName);
        if (optionMap.ContainsKey(choiceName))
        {
            var choice = optionMap[choiceName];
            choice.Advance.Invoke();
        }
    }

    public void AllFound()
    {
        runner.SetProperty("ȫ���ҵ�", true);
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
            case "��ʱ":
                DialogUIController.Instance.Hide();
                float delayTime = float.Parse(evt.Value);
                StartCoroutine(Delay(delayTime, evt.Advance));
                break;
            case "���ż�����Ч":
                AudioManager.Instance.PlayMemoryAudio(evt.Value);
                evt.Advance.Invoke();
                break;
            case "��ͣ������Ч":
                AudioManager.Instance.PauseMemoryAudio();
                evt.Advance.Invoke();
                break;
            case "����������Ч":
                AudioManager.Instance.ContinueMemoryAudio();
                evt.Advance.Invoke();
                break;
            case "����ѭ����Ч":
                AudioManager.Instance.PlayLoopAudio(evt.Value);
                evt.Advance.Invoke();
                break;
            case "��ͣѭ����Ч":
                AudioManager.Instance.PauseLoopAudio();
                evt.Advance.Invoke();
                break;
            case "����ѭ����Ч":
                AudioManager.Instance.ContinueLoopAudio();
                evt.Advance.Invoke();
                break;
            case "���Ž�����Ч":
                AudioManager.Instance.RandomPlayInteraction(evt.Value);
                evt.Advance.Invoke();
                break;
            case "�л�����ͼƬ":
                GuestController.Instance.ChangeGuestPic(evt.Actor.name, evt.Value);
                evt.Advance.Invoke();
                break;
            case "���ﵭ��":
                if (evt.Actor is DialogActor dialogActor)
                {
                    GuestController.Instance.GuestFadeIn(dialogActor.FileName, evt.Value).onComplete += () => evt.Advance.Invoke();
                }
                else
                {
                    GuestController.Instance.GuestFadeIn().onComplete += () => evt.Advance.Invoke();
                }
                break;
            case "���ﵭ��":
                GuestController.Instance.GuestFadeOut().onComplete += () => evt.Advance.Invoke();
                break;
            case "�л�����":
                DialogUIController.Instance.Hide();
                GameManager.Instance.SceneChange(evt.Value);
                tempAction = evt.Advance;
                break;
            case "չʾͼƬ":
                DialogUIController.Instance.Hide();
                Debug.Log("ͼƬչʾռλ");
                evt.Advance.Invoke();
                break;
            case "�����л�":
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
            Debug.Log("���в��");
            DialogUIController.Instance.Hide();
            // TODO �򿪽��в�����
            DisassemblyManager.Instance.StartRepairMode(false);
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
        else if (userEvent.Name == "���ֵ���")
        {
            FadeCanvas.Instance.FadeOut(1f).onComplete += () =>
            {
                userEvent.Advance.Invoke();
            };
        }
        else if (userEvent.Name == "�������ά��")
        {
            tempAction = userEvent.Advance;
            GameManager.Instance.DeepFix();
        }
        else if (userEvent.Name == "���ά�޲��")
        {
            DialogUIController.Instance.Hide();
            tempAction = userEvent.Advance;
            DisassemblyManager.Instance.StartRepairMode(true);
        }
        else if (userEvent.Name == "���ά�����")
        {
            // FIX ������Maze
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
        else if (userEvent.Name == "��������")
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
            // �Ի�ʱ��������
            AudioManager.Instance.RandomPlayVoice(dialogActor.FileName, dialogActor.AudioNum);
        }
    }

    private void HandleActorChoiceEvent(ActorChoiceEvent evt)
    {
        var actorName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        DialogUIController.Instance.ShowChoice(actorName, evt.Message, evt.Options);

        if (evt.Actor is DialogActor dialogActor)
        {
            // �Ի�ʱ��������
            AudioManager.Instance.RandomPlayVoice(dialogActor.FileName, dialogActor.AudioNum);
        }
    }

    private void HandleEnd()
    {
        Debug.Log("�Ի���ɣ����ضԻ���");
        DialogUIController.Instance.Hide();
        GameManager.Instance.NextTalk();
    }

    private IEnumerator Delay(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }
}



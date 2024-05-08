using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    [Header("��һ���Ի�")]
    [SerializeField] private TalkSceneSO firstTalkSO;
    
    private TalkSceneSO currentTalkSceneSO;

    [Header("�Ի�����")]
    [SerializeField] private AssetReference talkScene;

    [Header("���ά�޳���")]
    [SerializeField] private AssetReference deepFixScene;

    [Header("���߳���")]
    [SerializeField] private AssetReference windowScene;

    [Header("�¼�����")]
    // ����㲥
    [SerializeField] private SceneLoadEventSO loadEventSO;

    // ���ܼ���
    [SerializeField] private LoadStateEventSO afterSceneLoadEventSO;

    [SerializeField] private FadeEventSO fadeEventSO;

    [Header("���г���")]
    [SerializeField] private List<TalkSceneSO> talkScenes;


    private int fixIndex = 0;

    private void Start()
    {
        // ���ضԻ�����
        loadEventSO?.RaiseEvent(talkScene, false, LoadState.Menu);
    }

    private void OnEnable()
    {
        afterSceneLoadEventSO.OnStartTalkEventRaised += OnStartTalkLoaded;
        afterSceneLoadEventSO.OnContinueTalkEventRaised += OnContinueTalkLoaded;
        afterSceneLoadEventSO.OnFixEventRaised += OnFixLoaded;
        afterSceneLoadEventSO.OnDeepFixEventRaised += OnDeepFixLoad;
        afterSceneLoadEventSO.OnWindowEventRaised += OnLookAtWindowLoad;
        afterSceneLoadEventSO.OnNewDayEventRaised += OnNewDayLoad;
    }

    private void OnDisable()
    {
        afterSceneLoadEventSO.OnStartTalkEventRaised -= OnStartTalkLoaded;
        afterSceneLoadEventSO.OnContinueTalkEventRaised -= OnContinueTalkLoaded;
        afterSceneLoadEventSO.OnFixEventRaised -= OnFixLoaded;
        afterSceneLoadEventSO.OnDeepFixEventRaised -= OnDeepFixLoad;
        afterSceneLoadEventSO.OnWindowEventRaised -= OnLookAtWindowLoad;
        afterSceneLoadEventSO.OnNewDayEventRaised -= OnNewDayLoad;
    }

    public void StartNewGame()
    {
        // ��ť����
        Menu.Instance.TextFadeOut().onComplete += () =>
        {
            currentTalkSceneSO = firstTalkSO;
            StartTalk();
        };
    }

    public void StartNewGame(int startLevel)
    {
        Debug.Log(startLevel);
        Debug.Log(talkScenes[startLevel].name);
        // ��ť����
        Menu.Instance.TextFadeOut().onComplete += () =>
        {
            currentTalkSceneSO = talkScenes[startLevel];
            StartTalk();
        };
    }

    /// <summary>
    /// �����¶Ի�
    /// </summary>
    public void StartTalk()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_room");
        // ���ضԻ�����
        DialogUIController.Instance.SwitchDialog(DialogTpye.Normal);
        ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
    }

    /// <summary>
    /// ��ʼ�Ի�������ɺ󴥷�
    /// </summary>
    private void OnStartTalkLoaded()
    {
        // ��ʼ�Ի�
        ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
    }

    /// <summary>
    /// ά����ɺ�����Ի�
    /// </summary>
    public void ContinueTalk()
    {
        AudioManager.Instance.FadeOutAmb2Audio();
        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.DeleteMaze();
        }
        // ���¼��ضԻ�����
        DialogUIController.Instance.SwitchDialog(DialogTpye.Normal);
        loadEventSO?.RaiseEvent(talkScene, true, LoadState.ContinueTalk);
    }

    /// <summary>
    /// �����Ի�����������ɺ󴥷�
    /// </summary>
    private void OnContinueTalkLoaded()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_room");
        // �������
        GuestController.Instance.ShowGuest(currentTalkSceneSO.mainActor.FileName);
        
        // �����Ի�
        ConversationController.Instance.ContinueConversation();
    }

    /// <summary>
    /// ����ά��С��Ϸ
    /// </summary>
    public void Fix()
    {
        AudioManager.Instance.PauseAmb2Audio();
        AudioManager.Instance.RandomPlayInteraction("main_in");
        // ת��ά�޳���
        DialogUIController.Instance.SwitchDialog(DialogTpye.Head);
        loadEventSO?.RaiseEvent(currentTalkSceneSO.fixSceneReference[fixIndex], true, LoadState.Fix);
        fixIndex++;
    }

    private void OnFixLoaded()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_main");
        // ����������ɣ��Ի�����
        ConversationController.Instance.ContinueConversation();
    }

    public void DeepFix()
    {
        AudioManager.Instance.RandomPlayInteraction("deep_in");
        DialogUIController.Instance.SwitchDialog(DialogTpye.Head);
        // �������ά�޳���
        loadEventSO?.RaiseEvent(deepFixScene, true, LoadState.DeepFix);
    }

    private void OnDeepFixLoad()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_deep");
        // ���������
        IDeepRepairRule currentRule = DeepFixRuleFactory(currentTalkSceneSO.deepFixRule);
        DeepRepairManager.Instance.startReapir(currentRule, currentTalkSceneSO.deepFixPartName);

        // �����Ի�
        ConversationController.Instance.ContinueConversation();
    }

    /// <summary>
    /// ������һ�ζԻ�
    /// </summary>
    public void NextTalk()
    {
        if (currentTalkSceneSO.nextScene == null)
        {
            // TODO �˴���Ϸ����
            GameEnd();
            return;
        }

        // �ж��Ƿ������һ��
        if (!currentTalkSceneSO.isEndOfDay)
        {
            currentTalkSceneSO = currentTalkSceneSO.nextScene;
            FadeCanvas.Instance.FadeOut(1f).onComplete += () =>
            {
                ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
            };
            fixIndex = 0;
        } 
        else if (currentTalkSceneSO.windowType != "��С��")
        {
            // ��������
            Window.Instance.DropRope();
        }
    }

    public void LookAtWindow()
    {
        loadEventSO?.RaiseEvent(windowScene, true, LoadState.LookAtWindow);
    }

    private void OnLookAtWindowLoad()
    {
        Window.Instance.StartAnimation(currentTalkSceneSO.windowType);
        ConversationController.Instance.ContinueConversation();
    }

    public void NewDay()
    {
        FadeCanvas.Instance.FadeIn(1f).onComplete += () =>
        {
            loadEventSO?.RaiseEvent(talkScene, false, LoadState.NewDay);
            currentTalkSceneSO = currentTalkSceneSO.nextScene;
            Menu.Instance.FadeIn(0.1f);
            fixIndex = 0;
            ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
        };
    }

    private void OnNewDayLoad()
    {
        FadeCanvas.Instance.FadeOut(0.5f);
    }

    private IDeepRepairRule DeepFixRuleFactory(DeepRepairRuleType type)
    {
        if (type == DeepRepairRuleType.Rule1)
        {
            return new Rule1();
        } 
        else
        {
            return new Rule2();
        }
    }

    public void SceneChange(string sceneType)
    {
        switch (sceneType)
        {
            case "ά��":
                Fix();
                break;
            case "�Ի�":
                ContinueTalk();
                break;
            case "���ά��":
                DeepFix();
                break;
        }
    }

    public void GameEnd()
    {
        // ���ֵ���
        FadeCanvas.Instance.FadeIn(3f).onComplete += () =>
        {
            // ��ʾ��л����
            FadeCanvas.Instance.ShowThanksText();
        };
    }
}

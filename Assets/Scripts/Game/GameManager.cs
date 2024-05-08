using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    [Header("第一个对话")]
    [SerializeField] private TalkSceneSO firstTalkSO;
    
    private TalkSceneSO currentTalkSceneSO;

    [Header("对话场景")]
    [SerializeField] private AssetReference talkScene;

    [Header("深度维修场景")]
    [SerializeField] private AssetReference deepFixScene;

    [Header("床边场景")]
    [SerializeField] private AssetReference windowScene;

    [Header("事件挂载")]
    // 发起广播
    [SerializeField] private SceneLoadEventSO loadEventSO;

    // 接受监听
    [SerializeField] private LoadStateEventSO afterSceneLoadEventSO;

    [SerializeField] private FadeEventSO fadeEventSO;

    [Header("所有场景")]
    [SerializeField] private List<TalkSceneSO> talkScenes;


    private int fixIndex = 0;

    private void Start()
    {
        // 加载对话场景
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
        // 按钮淡出
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
        // 按钮淡出
        Menu.Instance.TextFadeOut().onComplete += () =>
        {
            currentTalkSceneSO = talkScenes[startLevel];
            StartTalk();
        };
    }

    /// <summary>
    /// 开启新对话
    /// </summary>
    public void StartTalk()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_room");
        // 加载对话场景
        DialogUIController.Instance.SwitchDialog(DialogTpye.Normal);
        ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
    }

    /// <summary>
    /// 开始对话加载完成后触发
    /// </summary>
    private void OnStartTalkLoaded()
    {
        // 开始对话
        ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
    }

    /// <summary>
    /// 维修完成后继续对话
    /// </summary>
    public void ContinueTalk()
    {
        AudioManager.Instance.FadeOutAmb2Audio();
        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.DeleteMaze();
        }
        // 重新加载对话场景
        DialogUIController.Instance.SwitchDialog(DialogTpye.Normal);
        loadEventSO?.RaiseEvent(talkScene, true, LoadState.ContinueTalk);
    }

    /// <summary>
    /// 继续对话场景加载完成后触发
    /// </summary>
    private void OnContinueTalkLoaded()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_room");
        // 人物出现
        GuestController.Instance.ShowGuest(currentTalkSceneSO.mainActor.FileName);
        
        // 继续对话
        ConversationController.Instance.ContinueConversation();
    }

    /// <summary>
    /// 进行维修小游戏
    /// </summary>
    public void Fix()
    {
        AudioManager.Instance.PauseAmb2Audio();
        AudioManager.Instance.RandomPlayInteraction("main_in");
        // 转到维修场景
        DialogUIController.Instance.SwitchDialog(DialogTpye.Head);
        loadEventSO?.RaiseEvent(currentTalkSceneSO.fixSceneReference[fixIndex], true, LoadState.Fix);
        fixIndex++;
    }

    private void OnFixLoaded()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_main");
        // 场景加载完成，对话继续
        ConversationController.Instance.ContinueConversation();
    }

    public void DeepFix()
    {
        AudioManager.Instance.RandomPlayInteraction("deep_in");
        DialogUIController.Instance.SwitchDialog(DialogTpye.Head);
        // 进入深度维修场景
        loadEventSO?.RaiseEvent(deepFixScene, true, LoadState.DeepFix);
    }

    private void OnDeepFixLoad()
    {
        AudioManager.Instance.PlayAmb2Audio("amb_deep");
        // 先生成深度
        IDeepRepairRule currentRule = DeepFixRuleFactory(currentTalkSceneSO.deepFixRule);
        DeepRepairManager.Instance.startReapir(currentRule, currentTalkSceneSO.deepFixPartName);

        // 继续对话
        ConversationController.Instance.ContinueConversation();
    }

    /// <summary>
    /// 进入下一段对话
    /// </summary>
    public void NextTalk()
    {
        if (currentTalkSceneSO.nextScene == null)
        {
            // TODO 此处游戏结束
            GameEnd();
            return;
        }

        // 判断是否是最后一天
        if (!currentTalkSceneSO.isEndOfDay)
        {
            currentTalkSceneSO = currentTalkSceneSO.nextScene;
            FadeCanvas.Instance.FadeOut(1f).onComplete += () =>
            {
                ConversationController.Instance.StartConversation(currentTalkSceneSO.conversation);
            };
            fixIndex = 0;
        } 
        else if (currentTalkSceneSO.windowType != "生小孩")
        {
            // 窗帘拉下
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
            case "维修":
                Fix();
                break;
            case "对话":
                ContinueTalk();
                break;
            case "深度维修":
                DeepFix();
                break;
        }
    }

    public void GameEnd()
    {
        // 遮罩淡入
        FadeCanvas.Instance.FadeIn(3f).onComplete += () =>
        {
            // 显示感谢游玩
            FadeCanvas.Instance.ShowThanksText();
        };
    }
}

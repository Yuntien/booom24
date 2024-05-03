using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("第一个对话场景")]
    [SerializeField] private TalkSceneSO firstTalkSO;
    
    private TalkSceneSO currentTalkSceneSO;

    [Header("事件挂载")]
    // 发起广播
    [SerializeField] private SceneLoadEventSO loadEventSO;

    // 接受监听
    [SerializeField] private LoadStateEventSO afterSceneLoadEventSO;

    private void Start()
    {
        // 加载对话场景
        loadEventSO?.RaiseEvent(firstTalkSO.sceneReference, false, LoadState.Menu);
    }

    private void OnEnable()
    {
        afterSceneLoadEventSO.OnStartTalkEventRaised += OnStartTalkLoaded;
        afterSceneLoadEventSO.OnContinueTalkEventRaised += OnContinueTalkLoaded;
        afterSceneLoadEventSO.OnFixEventRaised += OnFixLoaded;
    }

    private void OnDisable()
    {
        afterSceneLoadEventSO.OnStartTalkEventRaised -= OnStartTalkLoaded;
        afterSceneLoadEventSO.OnContinueTalkEventRaised -= OnContinueTalkLoaded;
        afterSceneLoadEventSO.OnFixEventRaised -= OnFixLoaded;
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

    /// <summary>
    /// 开启新对话
    /// </summary>
    public void StartTalk()
    {
        // 加载对话场景
        // loadEventSO?.RaiseEvent(currentTalkSceneSO.sceneReference, true, LoadState.StartTalk);
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
        // 重新加载对话场景
        loadEventSO?.RaiseEvent(currentTalkSceneSO.sceneReference, true, LoadState.ContinueTalk);
    }

    /// <summary>
    /// 继续对话场景加载完成后触发
    /// </summary>
    private void OnContinueTalkLoaded()
    {
        // 人物出现

        // 继续对话
        ConversationController.Instance.ContinueConversation();
    }

    /// <summary>
    /// 进行维修小游戏
    /// </summary>
    public void Fix()
    {
        // 转到维修场景
        loadEventSO?.RaiseEvent(currentTalkSceneSO.fixSceneReference, true, LoadState.Fix);
    }

    private void OnFixLoaded()
    {
        // 场景加载完成，对话继续
        ConversationController.Instance.ContinueConversation();
    }
}

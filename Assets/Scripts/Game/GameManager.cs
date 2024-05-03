using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("��һ���Ի�����")]
    [SerializeField] private TalkSceneSO firstTalkSO;
    
    private TalkSceneSO currentTalkSceneSO;

    [Header("�¼�����")]
    // ����㲥
    [SerializeField] private SceneLoadEventSO loadEventSO;

    // ���ܼ���
    [SerializeField] private LoadStateEventSO afterSceneLoadEventSO;

    private void Start()
    {
        // ���ضԻ�����
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
        // ��ť����
        Menu.Instance.TextFadeOut().onComplete += () =>
        {
            currentTalkSceneSO = firstTalkSO;
            StartTalk();
        };
    }

    /// <summary>
    /// �����¶Ի�
    /// </summary>
    public void StartTalk()
    {
        // ���ضԻ�����
        // loadEventSO?.RaiseEvent(currentTalkSceneSO.sceneReference, true, LoadState.StartTalk);
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
        // ���¼��ضԻ�����
        loadEventSO?.RaiseEvent(currentTalkSceneSO.sceneReference, true, LoadState.ContinueTalk);
    }

    /// <summary>
    /// �����Ի�����������ɺ󴥷�
    /// </summary>
    private void OnContinueTalkLoaded()
    {
        // �������

        // �����Ի�
        ConversationController.Instance.ContinueConversation();
    }

    /// <summary>
    /// ����ά��С��Ϸ
    /// </summary>
    public void Fix()
    {
        // ת��ά�޳���
        loadEventSO?.RaiseEvent(currentTalkSceneSO.fixSceneReference, true, LoadState.Fix);
    }

    private void OnFixLoaded()
    {
        // ����������ɣ��Ի�����
        ConversationController.Instance.ContinueConversation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    [Header("�˵�")]
    [SerializeField] private AssetReference menuScene;

    [Header("��һ���Ի�����")]
    [SerializeField] private TalkSceneSO firstTalkSO;
    
    private TalkSceneSO currentTalkSceneSO;

    // ����㲥
    [SerializeField] private SceneLoadEventSO loadEventSO;

    // ���ܼ���
    [SerializeField] private LoadStateEventSO afterSceneLoadEventSO;

    void Start()
    {
        loadEventSO?.RaiseEvent(menuScene, false, LoadState.Menu);
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
        currentTalkSceneSO = firstTalkSO;
        StartTalk();
    }

    /// <summary>
    /// �����¶Ի�
    /// </summary>
    public void StartTalk()
    {
        // ���ضԻ�����
        loadEventSO?.RaiseEvent(currentTalkSceneSO.sceneReference, true, LoadState.StartTalk);
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

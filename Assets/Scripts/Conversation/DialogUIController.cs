using Conversa.Runtime.Events;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : Singleton<DialogUIController>
{
    private GameObject curMessageWindow;
    private Text curActorNameText;
    private Text curMessageText;
    private Button curNextMessageButton;
    private GameObject curChoiceBox;
    private Image curHeadImage;
    [Header("��ͨ�Ի����")]
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private Text actorNameText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button nextMessageButton;
    [SerializeField] private GameObject choiceBox;
    [Header("��ͷ��Ի����")]
    [SerializeField] private GameObject messageWindowHead;
    [SerializeField] private Text actorNameTextHead;
    [SerializeField] private Text messageTextHead;
    [SerializeField] private Button nextMessageButtonHead;
    [SerializeField] private GameObject choiceBoxHead;
    [SerializeField] private Image headImage;
    [Header("��ťԤ����")]
    [SerializeField] private GameObject choiceOptionButtonPrefab;
    [Header("������ʾ�ٶ�")]
    [SerializeField] private float textSpeed = 0.05f;
    [Header("�հ�ͷ��")]
    [SerializeField] private Sprite emptyHead;

    private bool textIsPlaying;
    private Tweener textTweener;
    private Action nextStep;

    /// <summary>
    /// ��ʾ��Ϣ
    /// </summary>
    /// <param name="actorName">��������</param>
    /// <param name="message">��Ϣ</param>
    /// <param name="onContinue">��һ������Ϊ</param>
    public void ShowMessage(string actorName, string message, Action onContinue, Sprite head)
    {
        // ��ʾ�Ի����
        curMessageWindow.SetActive(true);
        curNextMessageButton.gameObject.SetActive(false);
        curChoiceBox.SetActive(false);

        // �����ı�
        curActorNameText.text = actorName;
        curMessageText.text = "";

        if (curHeadImage != null)
        {
            headImage.sprite = head == null ? emptyHead : head;
        }

        textIsPlaying = true;
        textTweener = curMessageText.DOText(message, message.Length * textSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            AudioManager.Instance.StopVoice();
            Invoke("TextPlayingEnd", 0.5f);
            curNextMessageButton.gameObject.SetActive(true);
            // �洢��һ���Ķ���
            nextStep = onContinue;
        });
    }

    /// <summary>
    /// ��ʾѡ��
    /// </summary>
    /// <param name="actorName">������</param>
    /// <param name="message">��Ϣ</param>
    /// <param name="options">ѡ��</param>
    public void ShowChoice(string actorName, string message, List<Option> options, Sprite head)
    {
        // ��ʾ���
        curMessageWindow.SetActive(true);
        curChoiceBox.SetActive(true);
        curNextMessageButton.gameObject.SetActive(false);

        // �����ı�
        curActorNameText.text = actorName;
        curMessageText.text = message;
        if (curHeadImage != null)
        {
            headImage.sprite = head == null ? emptyHead : head;
        }

        // ���ѡ������������
        foreach (Transform child in curChoiceBox.transform)
        {
            Destroy(child.gameObject);
        }

        // ���ѡ����
        options.ForEach(option =>
        {
            var instance = Instantiate(choiceOptionButtonPrefab, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(curChoiceBox.transform);
            instance.GetComponentInChildren<Text>().text = option.Message;
            instance.GetComponent<Button>().onClick.AddListener(() => option.Advance());
        });
    }

    /// <summary>
    /// ���ضԻ���
    /// </summary>
    public void Hide()
    {
        curMessageWindow.SetActive(false);
    }

    private void Update()
    {
        // ����¼�
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (textIsPlaying)
            {
                textTweener?.Complete();
            }
            else if (nextStep != null)
            {
                nextStep?.Invoke();
                nextStep = null;
            }
        }
    }

    public void SwitchDialog(DialogTpye type)
    {
        if (type == DialogTpye.Head)
        {
            messageWindow.gameObject.SetActive(false);
            curMessageWindow = messageWindowHead;
            curActorNameText = actorNameTextHead;
            curMessageText = messageTextHead;
            curNextMessageButton = nextMessageButtonHead;
            curChoiceBox = choiceBoxHead;
            curHeadImage = headImage;
        }
        else
        {
            messageWindowHead.gameObject.SetActive(false);
            curMessageWindow = messageWindow;
            curActorNameText = actorNameText;
            curMessageText = messageText;
            curNextMessageButton = nextMessageButton;
            curChoiceBox = choiceBox;
            curHeadImage = null;
        }
    }

    #region ����������
    private void TextPlayingEnd()
    {
        textIsPlaying = false;
    }
    #endregion
}

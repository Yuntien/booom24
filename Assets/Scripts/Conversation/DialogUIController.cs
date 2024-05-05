using Conversa.Runtime.Events;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : Singleton<DialogUIController>
{
    [Header("��Ϣ���")]
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private Text actorNameText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button nextMessageButton;
    [SerializeField] private GameObject choiceBox;
    [Header("��ťԤ����")]
    [SerializeField] private GameObject choiceOptionButtonPrefab;
    [Header("������ʾ�ٶ�")]
    [SerializeField] private float textSpeed = 0.05f;

    private bool textIsPlaying;
    private Tweener textTweener;
    private Action nextStep;

    /// <summary>
    /// ��ʾ��Ϣ
    /// </summary>
    /// <param name="actorName">��������</param>
    /// <param name="message">��Ϣ</param>
    /// <param name="onContinue">��һ������Ϊ</param>
    public void ShowMessage(string actorName, string message, Action onContinue)
    {
        // ��ʾ�Ի����
        messageWindow.SetActive(true);
        nextMessageButton.gameObject.SetActive(false);
        choiceBox.SetActive(false);

        // �����ı�
        actorNameText.text = actorName;
        messageText.text = "";
        textIsPlaying = true;
        textTweener = messageText.DOText(message, message.Length * textSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            Invoke("TextPlayingEnd", 0.5f);
            nextMessageButton.gameObject.SetActive(true);
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
    public void ShowChoice(string actorName, string message, List<Option> options)
    {
        // ��ʾ���
        messageWindow.SetActive(true);
        choiceBox.SetActive(true);
        nextMessageButton.gameObject.SetActive(false);

        // �����ı�
        actorNameText.text = actorName;
        messageText.text = message;

        // ���ѡ������������
        foreach (Transform child in choiceBox.transform)
        {
            Destroy(child.gameObject);
        }

        // ���ѡ����
        options.ForEach(option =>
        {
            var instance = Instantiate(choiceOptionButtonPrefab, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(choiceBox.transform);
            instance.GetComponentInChildren<Text>().text = option.Message;
            instance.GetComponent<Button>().onClick.AddListener(() => option.Advance());
        });
    }

    /// <summary>
    /// ���ضԻ���
    /// </summary>
    public void Hide()
    {
        messageWindow.SetActive(false);
    }

    private void Update()
    {
        // ����¼�
        if (Input.GetMouseButtonDown(0))
        {
            if (textIsPlaying)
            {
                textTweener?.Complete();
            }
            else if (nextStep != null)
            {
                nextStep?.Invoke();
                nextStep = null;
                // ���ʱ��ֹ����
                AudioManager.Instance.StopVoice();
            }
        }
    }

    #region ����������
    private void TextPlayingEnd()
    {
        textIsPlaying = false;
    }
    #endregion
}

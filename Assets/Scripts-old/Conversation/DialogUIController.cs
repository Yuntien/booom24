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

        // �����ı�
        actorNameText.text = actorName;
        messageText.text = message;

        // ��һ����ť����
        nextMessageButton.enabled = true;
        nextMessageButton.onClick.RemoveAllListeners();
        nextMessageButton.onClick.AddListener(() => onContinue());
    }

    /// <summary>
    /// ���ضԻ���
    /// </summary>
    public void Hide()
    {
        messageWindow.SetActive(false);
    }
}

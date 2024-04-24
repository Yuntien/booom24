using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : Singleton<DialogUIController>
{
    [Header("信息面板")]
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private Text actorNameText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button nextMessageButton;

    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="actorName">人物名称</param>
    /// <param name="message">信息</param>
    /// <param name="onContinue">下一步的行为</param>
    public void ShowMessage(string actorName, string message, Action onContinue)
    {
        // 显示对话面板
        messageWindow.SetActive(true);

        // 更新文本
        actorNameText.text = actorName;
        messageText.text = message;

        // 下一步按钮监听
        nextMessageButton.enabled = true;
        nextMessageButton.onClick.RemoveAllListeners();
        nextMessageButton.onClick.AddListener(() => onContinue());
    }

    /// <summary>
    /// 隐藏对话框
    /// </summary>
    public void Hide()
    {
        messageWindow.SetActive(false);
    }
}

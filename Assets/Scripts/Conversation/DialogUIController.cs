using Conversa.Runtime.Events;
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
    [SerializeField] private GameObject choiceBox;
    [Header("按钮预制体")]
    [SerializeField] private GameObject choiceOptionButtonPrefab;

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
        nextMessageButton.gameObject.SetActive(true);
        choiceBox.SetActive(false);

        // 更新文本
        actorNameText.text = actorName;
        messageText.text = message;

        // 下一步按钮监听
        nextMessageButton.enabled = true;
        nextMessageButton.onClick.RemoveAllListeners();
        nextMessageButton.onClick.AddListener(() => onContinue());
    }

    /// <summary>
    /// 显示选择
    /// </summary>
    /// <param name="actorName">人物名</param>
    /// <param name="message">消息</param>
    /// <param name="options">选项</param>
    public void ShowChoice(string actorName, string message, List<Option> options)
    {
        // 显示组件
        messageWindow.SetActive(true);
        choiceBox.SetActive(true);
        nextMessageButton.gameObject.SetActive(false);

        // 更新文本
        actorNameText.text = actorName;
        messageText.text = message;

        // 清除选择面板的子物体
        foreach (Transform child in choiceBox.transform)
        {
            Destroy(child.gameObject);
        }

        // 添加选择项
        options.ForEach(option =>
        {
            var instance = Instantiate(choiceOptionButtonPrefab, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(choiceBox.transform);
            instance.GetComponentInChildren<Text>().text = option.Message;
            instance.GetComponent<Button>().onClick.AddListener(() => option.Advance());
        });
    }

    /// <summary>
    /// 隐藏对话框
    /// </summary>
    public void Hide()
    {
        messageWindow.SetActive(false);
    }
}

using Conversa.Runtime.Events;
using DG.Tweening;
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
    [Header("文字显示速度")]
    [SerializeField] private float textSpeed = 0.05f;

    private bool textIsPlaying;
    private Tweener textTweener;
    private Action nextStep;

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
        nextMessageButton.gameObject.SetActive(false);
        choiceBox.SetActive(false);

        // 更新文本
        actorNameText.text = actorName;
        messageText.text = "";
        textIsPlaying = true;
        textTweener = messageText.DOText(message, message.Length * textSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            Invoke("TextPlayingEnd", 0.5f);
            nextMessageButton.gameObject.SetActive(true);
            // 存储下一步的动作
            nextStep = onContinue;
        });
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

    private void Update()
    {
        // 点击事件
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
                // 点击时终止语音
                AudioManager.Instance.StopVoice();
            }
        }
    }

    #region 解除点击阻塞
    private void TextPlayingEnd()
    {
        textIsPlaying = false;
    }
    #endregion
}

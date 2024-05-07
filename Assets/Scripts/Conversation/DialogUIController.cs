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
    [Header("普通对话面板")]
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private Text actorNameText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button nextMessageButton;
    [SerializeField] private GameObject choiceBox;
    [Header("带头像对话面板")]
    [SerializeField] private GameObject messageWindowHead;
    [SerializeField] private Text actorNameTextHead;
    [SerializeField] private Text messageTextHead;
    [SerializeField] private Button nextMessageButtonHead;
    [SerializeField] private GameObject choiceBoxHead;
    [SerializeField] private Image headImage;
    [Header("按钮预制体")]
    [SerializeField] private GameObject choiceOptionButtonPrefab;
    [Header("文字显示速度")]
    [SerializeField] private float textSpeed = 0.05f;
    [Header("空白头像")]
    [SerializeField] private Sprite emptyHead;

    private bool textIsPlaying;
    private Tweener textTweener;
    private Action nextStep;

    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="actorName">人物名称</param>
    /// <param name="message">信息</param>
    /// <param name="onContinue">下一步的行为</param>
    public void ShowMessage(string actorName, string message, Action onContinue, Sprite head)
    {
        // 显示对话面板
        curMessageWindow.SetActive(true);
        curNextMessageButton.gameObject.SetActive(false);
        curChoiceBox.SetActive(false);

        // 更新文本
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
    public void ShowChoice(string actorName, string message, List<Option> options, Sprite head)
    {
        // 显示组件
        curMessageWindow.SetActive(true);
        curChoiceBox.SetActive(true);
        curNextMessageButton.gameObject.SetActive(false);

        // 更新文本
        curActorNameText.text = actorName;
        curMessageText.text = message;
        if (curHeadImage != null)
        {
            headImage.sprite = head == null ? emptyHead : head;
        }

        // 清除选择面板的子物体
        foreach (Transform child in curChoiceBox.transform)
        {
            Destroy(child.gameObject);
        }

        // 添加选择项
        options.ForEach(option =>
        {
            var instance = Instantiate(choiceOptionButtonPrefab, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(curChoiceBox.transform);
            instance.GetComponentInChildren<Text>().text = option.Message;
            instance.GetComponent<Button>().onClick.AddListener(() => option.Advance());
        });
    }

    /// <summary>
    /// 隐藏对话框
    /// </summary>
    public void Hide()
    {
        curMessageWindow.SetActive(false);
    }

    private void Update()
    {
        // 点击事件
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

    #region 解除点击阻塞
    private void TextPlayingEnd()
    {
        textIsPlaying = false;
    }
    #endregion
}

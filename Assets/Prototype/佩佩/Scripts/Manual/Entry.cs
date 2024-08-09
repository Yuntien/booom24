using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class Entry
{
    private EyeTransitionManager eyeTransitionManager;

    public enum ButtonAction
    {
        ExecuteCode,
        EnterEyeView,
        EnterUFView,
        EnterMemoryView
        // 添加其他你需要的操作
    }

    public Button button;
    public ButtonAction buttonAction;
    public string codeDescription;  // 添加一个字段来存储代码的描述

    public void SetupButton(ManualManager manualManager, EyeTransitionManager eyeTransitionManager)
    {
        this.eyeTransitionManager = eyeTransitionManager; // Store a reference to the EyeTransitionManager

        button.onClick.AddListener(() =>
        {
            manualManager.ShowCode(this); // 修改这里，将当前条目传递给 ShowCode 方法
        });
    }
}

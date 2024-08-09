using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ManualManager : MonoBehaviour
{
    public List<Page> pages;
    public EyeTransitionManager eyeTransitionManager; 
    public GameObject manualGameObject; 
    public Button manualButton; 
    public GameObject executeWindow; 
    public TextMeshProUGUI executeWindowText; 
    public Button executeWindowButton; 
    public Button nextPageButton;  
    public Button previousPageButton;  

    private bool isManualVisible = false;
    private int currentIndex = 0;  
    private Entry currentEntry; // 添加一个变量来保存当前选中的条目

    private void Start()
    {
        foreach (var page in pages)
        {
            page.SetupPage(this, eyeTransitionManager);
            page.gameObject.SetActive(false);  // 初始化时隐藏所有页面
        }

        manualButton.onClick.AddListener(ToggleManual);
        nextPageButton.onClick.AddListener(NextPage);  
        previousPageButton.onClick.AddListener(PreviousPage);  
    }

    private void ToggleManual()
    {
        isManualVisible = !isManualVisible;
        manualGameObject.SetActive(isManualVisible);
        if (isManualVisible)
        {
            pages[currentIndex].gameObject.SetActive(true);  // 显示当前页面
        }
        else
        {
            foreach (var page in pages)
            {
                page.gameObject.SetActive(false);  // 隐藏所有页面
            }
        }
    }

    public void ShowCode(Entry entry) // 修改这个方法，使其接受一个 Entry 对象作为参数
    {
        currentEntry = entry; // 更新当前选中的条目
        executeWindow.SetActive(true);
        executeWindowText.text = entry.codeDescription; // 显示条目的代码描述
        executeWindowButton.onClick.AddListener(() =>
        {
            executeWindow.SetActive(false);
            ExecuteCode();  // 在这里添加执行代码的逻辑
        });
    }

    public void ExecuteCode()
    {
        if (currentEntry != null)
        {
            Debug.Log("Executing code: " + currentEntry.codeDescription);
            switch (currentEntry.buttonAction)
            {
                case Entry.ButtonAction.ExecuteCode:
                    // 在这里添加执行代码的逻辑
                    break;
                case Entry.ButtonAction.EnterEyeView:
                    eyeTransitionManager.RequestViewChange(EyeTransitionManager.ViewState.EyeView);
                    break;
                case Entry.ButtonAction.EnterUFView:
                    eyeTransitionManager.RequestViewChange(EyeTransitionManager.ViewState.UFView);
                    break;
                case Entry.ButtonAction.EnterMemoryView:
                    eyeTransitionManager.RequestViewChange(EyeTransitionManager.ViewState.MemoryView);
                    break;
            }
        }
        executeWindowButton.onClick.RemoveAllListeners(); // 执行完操作后，移除监听器
    }

    private void NextPage()
    {
        if (currentIndex < pages.Count - 1)
        {
            pages[currentIndex].gameObject.SetActive(false);  // 隐藏当前页面
            currentIndex++;
            pages[currentIndex].gameObject.SetActive(true);  // 显示新页面
        }
    }

    private void PreviousPage()
    {
        if (currentIndex > 0)
        {
            pages[currentIndex].gameObject.SetActive(false);  // 隐藏当前页面
            currentIndex--;
            pages[currentIndex].gameObject.SetActive(true);  // 显示新页面
        }
    }
}


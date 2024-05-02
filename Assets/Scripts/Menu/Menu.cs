using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : Singleton<Menu>
{
    [Header("菜单背景")]
    [SerializeField] private Image menuBackGround;
    [Header("菜单文字")]
    [SerializeField] private CanvasGroup menuText;

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ContinueGame()
    {
        // TODO 添加继续游戏逻辑
    }

    public void StartNewGame()
    {
        // TODO 开启新游戏
        GameManager.Instance.StartNewGame();
    }

    /// <summary>
    /// 文字淡出效果
    /// </summary>
    /// <returns>Dotween回调</returns>
    public Tweener TextFadeOut()
    {
        Tweener fadeCallBack = menuText.DOFade(0, 2);
        fadeCallBack.OnComplete(() =>
        {
            menuText.gameObject.SetActive(false);
        });
        return fadeCallBack;
    }
}

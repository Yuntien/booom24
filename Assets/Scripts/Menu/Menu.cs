using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Conversa.Runtime;
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

    public void TextFadeOut()
    {
        Debug.Log("Text Fade");
        menuText.DOFade(0, 2).OnComplete(() =>
        {
            menuText.gameObject.SetActive(false);
        });
        menuBackGround.DOBlendableColor(Color.clear, 2f).OnComplete(() => {
            menuBackGround.gameObject.SetActive(false);
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Conversa.Runtime;
using DG.Tweening;

public class Menu : Singleton<Menu>
{
    [Header("�˵�����")]
    [SerializeField] private Image menuBackGround;
    [Header("�˵�����")]
    [SerializeField] private CanvasGroup menuText;

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ContinueGame()
    {
        // TODO ��Ӽ�����Ϸ�߼�
    }

    public void StartNewGame()
    {
        // TODO ��������Ϸ
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

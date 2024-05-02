using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    /// <summary>
    /// ���ֵ���Ч��
    /// </summary>
    /// <returns>Dotween�ص�</returns>
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

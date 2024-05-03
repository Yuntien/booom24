using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : Singleton<Menu>
{
    [Header("�˵�����")]
    [SerializeField] private Image menuBackground;
    [Header("�˵�����")]
    [SerializeField] private CanvasGroup menuText;

    [Header("�����л��¼�")]
    [SerializeField] private FadeEventSO sceneFadeEventSO;

    private void OnEnable()
    {
        sceneFadeEventSO.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        sceneFadeEventSO.OnEventRaised -= OnFadeEvent;
    }

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

    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        menuBackground.DOBlendableColor(target, duration);
    }

    public Tweener FadeIn(float duration)
    {
        return menuBackground.DOBlendableColor(Color.black, duration);
    }

    public Tweener FadeOut(float duration)
    {
        return menuBackground.DOBlendableColor(Color.clear, duration);
    }
}
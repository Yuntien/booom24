using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanvas : Singleton<FadeCanvas>
{
    [Header("�¼�����")]
    public FadeEventSO fadeEvent;
    public Image fadeImage;

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        Debug.Log("����" + fadeIn);
        fadeImage.DOBlendableColor(target, duration);
    }

    public Tweener FadeIn(float duration)
    {
        Debug.Log("���ֵ���");
        return fadeImage.DOBlendableColor(Color.black, duration);
    }

    public Tweener FadeOut(float duration)
    {
        Debug.Log("���ֵ���");
        return fadeImage.DOBlendableColor(Color.clear, duration);
    }
}

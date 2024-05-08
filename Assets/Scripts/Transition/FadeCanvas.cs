using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class FadeCanvas : Singleton<FadeCanvas>
{
    [Header("�¼�����")]
    public FadeEventSO fadeEvent;
    public Image fadeImage;
    public TextMeshProUGUI thanksText;
    public GameObject returnMenu;

    [Header("�ڰ״���")]
    [SerializeField] private PostProcessVolume volume;

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

    public void LostColor()
    {
        volume.weight = 1;
    }

    public void ShowColor()
    {
        volume.weight = 0;
    }

    public void ShowThanksText()
    {
        thanksText.DOBlendableColor(Color.white, 1f);
        Invoke("ShowReturnMenu", 1);
    }

    private void ShowReturnMenu()
    {
        returnMenu.SetActive(true);
    }

    public void ReturnMenu()
    {
        thanksText.color = Color.clear;
        returnMenu.SetActive(false);
        
        Menu.Instance.FadeIn(0.1f);
        Menu.Instance.TextFadeIn().onComplete += () => {
            FadeOut(2);
        };
    }
}

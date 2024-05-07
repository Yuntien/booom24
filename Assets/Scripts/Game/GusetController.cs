using Conversa.Runtime;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestController : Singleton<GuestController>
{
    [Header("����")]
    [SerializeField] private SpriteRenderer guestSprite;
    [Header("����ʱ��")]
    [SerializeField] private float fadeTime = 1f;
    [Header("ά��λ��")]
    [SerializeField] private Transform fixPos;

    /// <summary>
    /// ���ﵭ��
    /// </summary>
    public Tweener GuestFadeOut()
    {
        return guestSprite.DOFade(0, fadeTime);
    }

    /// <summary>
    /// ���ﵭ��
    /// </summary>
    public Tweener GuestFadeIn(string actor, string picName)
    {
        if (picName != null && picName != "")
        {
            guestSprite.sprite = Resources.Load<Sprite>($"Art/{actor}/{picName}");
        }
        return guestSprite.DOFade(1, fadeTime);
    }

    /// <summary>
    /// ���ﵭ��
    /// </summary>
    public Tweener GuestFadeIn()
    {
        return guestSprite.DOFade(1, fadeTime);
    }

    public void ShowGuest(string name)
    {
        guestSprite.sprite = Resources.Load<Sprite>($"Art/{name}/1");
        ShowGuest();
    }

    /// <summary>
    /// ������ʾ
    /// </summary>
    public void ShowGuest()
    {
        // ����͸����
        Color currentColor = guestSprite.color;
        currentColor.a = 1;
        guestSprite.color = currentColor;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void HideGuest()
    {
        // ����͸����
        Color currentColor = guestSprite.color;
        currentColor.a = 0;
        guestSprite.color = currentColor;
    }

    /// <summary>
    /// �ƶ�������ά�޴�
    /// </summary>
    public Tweener MoveGuestToFix()
    {
        AudioManager.Instance.RandomPlayInteraction("walk");
        Tweener res = guestSprite.transform.DOMove(fixPos.position, 1f);
        res.onComplete = () =>
        {
            AudioManager.Instance.StopInteraction();
        };
        return res;
    }

    /// <summary>
    /// �л�����ͼƬ
    /// </summary>
    /// <param name="picLocation">ͼƬ�ļ���Resource�ļ����е�λ��</param>
    public void ChangeGuestPic(string actor, string picName)
    {
        guestSprite.sprite = Resources.Load<Sprite>($"Art/{actor}/{picName}");
    }
}

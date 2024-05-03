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
    public void GuestFadeOut()
    {
        guestSprite.DOFade(0, fadeTime);
    }

    /// <summary>
    /// ���ﵭ��
    /// </summary>
    public void GuestFadeIn()
    {
        guestSprite.DOFade(1, fadeTime);
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
        return guestSprite.transform.DOMove(fixPos.position, 1f);
    }

    /// <summary>
    /// �л�����ͼƬ
    /// </summary>
    /// <param name="picLocation">ͼƬ�ļ���Resource�ļ����е�λ��</param>
    public void ChangeGuestPic(string picLocation)
    {
        guestSprite.sprite = Resources.Load<Sprite>(picLocation);
    }
}

using Conversa.Runtime;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestController : Singleton<GuestController>
{
    [Header("客人")]
    [SerializeField] private SpriteRenderer guestSprite;
    [Header("淡出时间")]
    [SerializeField] private float fadeTime = 1f;
    [Header("维修位置")]
    [SerializeField] private Transform fixPos;

    /// <summary>
    /// 人物淡出
    /// </summary>
    public Tweener GuestFadeOut()
    {
        return guestSprite.DOFade(0, fadeTime);
    }

    /// <summary>
    /// 人物淡入
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
    /// 人物淡入
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
    /// 人物显示
    /// </summary>
    public void ShowGuest()
    {
        // 设置透明度
        Color currentColor = guestSprite.color;
        currentColor.a = 1;
        guestSprite.color = currentColor;
    }

    /// <summary>
    /// 人物隐藏
    /// </summary>
    public void HideGuest()
    {
        // 设置透明度
        Color currentColor = guestSprite.color;
        currentColor.a = 0;
        guestSprite.color = currentColor;
    }

    /// <summary>
    /// 移动人物至维修处
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
    /// 切换人物图片
    /// </summary>
    /// <param name="picLocation">图片文件在Resource文件夹中的位置</param>
    public void ChangeGuestPic(string actor, string picName)
    {
        guestSprite.sprite = Resources.Load<Sprite>($"Art/{actor}/{picName}");
    }
}

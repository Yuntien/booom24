using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundController : Singleton<BackgroundController>
{
    [Header("对话背景")]
    [SerializeField] private Transform backgorund;
    [Header("移动位置")]
    [SerializeField] private Transform movePos;
    [Header("缩小比例")]
    [SerializeField] private int scaleNum;

    public Sequence MoveToWindow()
    {
        AudioManager.Instance.RandomPlayInteraction("amb_window_1");
        var sequence = DOTween.Sequence();
        sequence.Append(backgorund.DOMove(-movePos.position, 5f));
        sequence.Join(backgorund.DOScale(new Vector3(scaleNum, scaleNum, scaleNum), 5f));
        sequence.onComplete += () =>
        {
            AudioManager.Instance.StopInteraction();
        };
        return sequence;
    }

    public void ResetBackground()
    {
        backgorund.position = Vector3.zero;
        backgorund.localScale = Vector3.one;
    }
}

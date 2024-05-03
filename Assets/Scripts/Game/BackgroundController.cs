using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using DG.Tweening;

public class BackgroundController : Singleton<BackgroundController>
{
    [Header("¶Ô»°±³¾°")]
    [SerializeField] private Transform backgorund;

    public Sequence MoveToWindow()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(backgorund.DOMove(new Vector3(0.8f, -2f, 0f), 5f));
        sequence.Join(backgorund.DOScale(new Vector3(2f, 2f, 2f), 5f));
        return sequence;
    }
}

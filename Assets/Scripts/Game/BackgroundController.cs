using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundController : Singleton<BackgroundController>
{
    [Header("�Ի�����")]
    [SerializeField] private Transform backgorund;
    [Header("�ƶ�λ��")]
    [SerializeField] private Transform movePos;
    [Header("��С����")]
    [SerializeField] private int scaleNum;

    public Sequence MoveToWindow()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(backgorund.DOMove(-movePos.position, 5f));
        sequence.Join(backgorund.DOScale(new Vector3(scaleNum, scaleNum, scaleNum), 5f));
        return sequence;
    }

    public void ResetBackground()
    {
        backgorund.position = Vector3.zero;
        backgorund.localScale = Vector3.one;
    }
}

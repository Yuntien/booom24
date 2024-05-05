using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Conversa/Dialog Actor", order = 2)]
public class DialogActor : Actor
{
    [Header("ͷ��")]
    [SerializeField] private Sprite avatar;
    public Sprite Avatar => avatar;

    [Header("��������")]
    [SerializeField] private int audioNum;
    public int AudioNum => audioNum;

    [Header("�ļ�λ��")]
    [SerializeField] private string fileName;
    public string FileName => fileName;
}

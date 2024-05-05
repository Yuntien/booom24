using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Conversa/Dialog Actor", order = 2)]
public class DialogActor : Actor
{
    [Header("头像")]
    [SerializeField] private Sprite avatar;
    public Sprite Avatar => avatar;

    [Header("语音数量")]
    [SerializeField] private int audioNum;
    public int AudioNum => audioNum;

    [Header("文件位置")]
    [SerializeField] private string fileName;
    public string FileName => fileName;
}

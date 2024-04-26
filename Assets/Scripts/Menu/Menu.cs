using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conversa.Runtime;

public class Menu : MonoBehaviour
{
    public string startScene;

    [SerializeField]
    private Conversation firstTalk;

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ContinueGame()
    {
        // TODO ��Ӽ�����Ϸ�߼�
    }

    public void StartNewGame()
    {
        // TODO ��������Ϸ
        GameManager.Instance.StartNewGame();
    }
}

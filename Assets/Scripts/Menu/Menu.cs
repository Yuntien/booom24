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
        // TODO 添加继续游戏逻辑
    }

    public void StartNewGame()
    {
        // TODO 开启新游戏
        GameManager.Instance.StartNewGame();
    }
}

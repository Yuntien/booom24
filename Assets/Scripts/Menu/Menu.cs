using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        TransitionManager.Instance.Transition("Menu", startScene);
        StartTalk();
    }

    private void StartTalk()
    {
        ConversationController.Instance.StartConversation(firstTalk);
    }

    public void GoBackToMenu()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "Menu");

        // TODO 保存游戏

    }
}

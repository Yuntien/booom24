using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string startScene;

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
    }

    public void GoBackToMenu()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "Menu");

        // TODO 保存游戏

    }
}

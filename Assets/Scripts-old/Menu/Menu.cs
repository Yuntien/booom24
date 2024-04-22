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
        // TODO ��Ӽ�����Ϸ�߼�
    }

    public void StartNewGame()
    {
        // TODO ��������Ϸ
        TransitionManager.Instance.Transition("Menu", startScene);
    }

    public void GoBackToMenu()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "Menu");

        // TODO ������Ϸ

    }
}

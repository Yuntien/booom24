using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    public CanvasGroup fadeCanvasGroup;

    public float fadeDuration;

    private bool isFade;

    /// <summary>
    /// ����ת��
    /// </summary>
    /// <param name="fromScene">��Դ����</param>
    /// <param name="toScene">Ŀ�곡��</param>
    public void Transition(string fromScene, string toScene)
    {
        if (!isFade)
        {
            StartCoroutine(TransToScene(fromScene, toScene));
        }
    }

    private IEnumerator TransToScene(string fromScene, string toScene)
    {
        yield return Fade(1);
        yield return SceneManager.UnloadSceneAsync(fromScene);
        yield return SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Additive);

        // �����³���Ϊ�����
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);

        yield return Fade(0);
    }

    /// <summary>
    /// ���ֵ��뵭��
    /// </summary>
    /// <param name="targetAlpha">1:�ڣ�0:͸��</param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;
        while(!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
    }
}

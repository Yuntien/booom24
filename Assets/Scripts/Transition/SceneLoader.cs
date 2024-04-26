using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("����ʱ��")]
    public float fadeDureation;

    [Header("�㲥")]
    public LoadStateEventSO afterSceneLoaderEvent;
    public FadeEventSO fadeEvent;
    public VoidEventSO sceneUnloadEvent;
    [Header("�¼�����")]
    public SceneLoadEventSO loadEvent;

    # region ����ת������
    private AssetReference currentScene;
    private AssetReference sceneToLoad;
    private bool showFadeScreen;
    private bool isLoading;
    private LoadState loadState;
    # endregion

    private void OnEnable()
    {
        loadEvent.OnEventRaised += LoadScene;
    }

    private void OnDisable()
    {
        loadEvent.OnEventRaised -= LoadScene;
    }

    private void LoadScene(AssetReference sceneToLoad, bool showFadeScreen, LoadState loadState)
    {
        if (isLoading) return;

        // ����ת������
        this.isLoading = true;
        this.sceneToLoad = sceneToLoad;
        this.showFadeScreen = showFadeScreen;
        this.loadState = loadState;

        if (currentScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnloadPreviousScene()
    {
        if (showFadeScreen)
        {
            fadeEvent.FadeIn(fadeDureation);
        }

        yield return new WaitForSeconds(fadeDureation);

        // TODO ����ж���¼� ��ʱ���ò���

        yield return currentScene.UnLoadScene();

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadComplated;
    }

    /// <summary>
    /// �������ؽ�����
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadComplated(AsyncOperationHandle<SceneInstance> handle)
    {
        currentScene = sceneToLoad;

        if (showFadeScreen)
        {
            //TODO
            fadeEvent.FadeOut(fadeDureation);
        }

        isLoading = false;

        afterSceneLoaderEvent?.RaiseEvent(loadState);
    }
}

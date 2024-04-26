using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("遮罩时间")]
    public float fadeDureation;

    [Header("广播")]
    public LoadStateEventSO afterSceneLoaderEvent;
    public FadeEventSO fadeEvent;
    public VoidEventSO sceneUnloadEvent;
    [Header("事件监听")]
    public SceneLoadEventSO loadEvent;

    # region 场景转换参数
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

        // 场景转换参数
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

        // TODO 场景卸载事件 暂时还用不到

        yield return currentScene.UnLoadScene();

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadComplated;
    }

    /// <summary>
    /// 场景加载结束后
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

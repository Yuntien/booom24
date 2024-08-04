using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EyeTransitionManager : MonoBehaviour
{
    public float zoomDuration = 1f;
    public Image blackScreenImage;
    public Button zoomInButton;
    public Button zoomOutButton;
    public GameObject eyePanel;
    public Image faceImage;

    private Material faceMaterial;
    private Vector2 normalViewScale = new Vector2(1f, 1f);
    private Vector2 normalViewPivot = new Vector2(0.5f, 0.5f);
    private Vector2 eyeViewScale = new Vector2(2f, 2f);
    private Vector2 eyeViewPivot = new Vector2(0.15f, 0.5f);

    private enum ViewState
    {
        NormalView,
        EyeView
    }

    private ViewState currentViewState = ViewState.NormalView;
    private bool isTransitioning = false;

    private void Start()
    {
        InitializeView();
        zoomInButton.onClick.AddListener(() => StartTransition(ViewState.EyeView));
        zoomOutButton.onClick.AddListener(() => StartTransition(ViewState.NormalView));
    }

    private void InitializeView()
    {
        faceMaterial = faceImage.material;
        blackScreenImage.gameObject.SetActive(false);
        SetView(currentViewState);
    }

    private void SetView(ViewState viewState)
    {
        Vector2 targetScale = viewState == ViewState.NormalView ? normalViewScale : eyeViewScale;
        Vector2 targetPivot = viewState == ViewState.NormalView ? normalViewPivot : eyeViewPivot;
        faceMaterial.SetVector("_UVScaleScale", targetScale);
        faceMaterial.SetVector("_UVScalePivot", targetPivot);
    }

      private void StartTransition(ViewState targetViewState)
{
    if (isTransitioning || currentViewState == targetViewState) return;
    isTransitioning = true;

    if (targetViewState == ViewState.EyeView)
    {
        // Zoom In 的时候，开始 Zoom
        Sequence zoomInSequence = DOTween.Sequence();
        zoomInSequence.Append(DOTween.To(() => faceMaterial.GetVector("_UVScaleScale"), x => faceMaterial.SetVector("_UVScaleScale", x), (Vector4)eyeViewScale, zoomDuration).SetEase(Ease.OutCubic));
        zoomInSequence.Join(DOTween.To(() => faceMaterial.GetVector("_UVScalePivot"), x => faceMaterial.SetVector("_UVScalePivot", x), (Vector4)eyeViewPivot, zoomDuration).SetEase(Ease.OutCubic));

        // 在 Zoom 到一半时开始黑屏过渡
        zoomInSequence.Insert(zoomDuration / 2, blackScreenImage.DOFade(1f, zoomDuration / 2).OnStart(() => blackScreenImage.gameObject.SetActive(true)));

        // 在整个 Zoom 完成后激活 Eye View
        zoomInSequence.OnComplete(() => ActivateEyeView());
    }
    else
    {
        // Zoom Out 的时候，先黑屏淡入
        blackScreenImage.gameObject.SetActive(true);
        Sequence zoomOutSequence = DOTween.Sequence();
        zoomOutSequence.Append(blackScreenImage.DOFade(1f, zoomDuration / 2));

        // 在黑屏淡入完成后关闭 Eye Panel
        zoomOutSequence.AppendCallback(() => eyePanel.SetActive(false));

        // 开始淡出黑屏
        zoomOutSequence.Append(blackScreenImage.DOFade(0f, zoomDuration).OnComplete(() => blackScreenImage.gameObject.SetActive(false)));

        // 在淡出黑屏到一半时开始 Zoom Out
        zoomOutSequence.Insert(zoomDuration / 2, DOTween.To(() => faceMaterial.GetVector("_UVScaleScale"), x => faceMaterial.SetVector("_UVScaleScale", x), (Vector4)normalViewScale, zoomDuration).SetEase(Ease.OutCubic));
        zoomOutSequence.Insert(zoomDuration / 2, DOTween.To(() => faceMaterial.GetVector("_UVScalePivot"), x => faceMaterial.SetVector("_UVScalePivot", x), (Vector4)normalViewPivot, zoomDuration).SetEase(Ease.OutCubic));

        // 在整个动画完成后更新视图状态
        zoomOutSequence.OnComplete(() =>
        {
            currentViewState = ViewState.NormalView;
            isTransitioning = false;
        });
    }
}




    private void StartZoom(Vector2 targetScale, Vector2 targetPivot, TweenCallback onComplete)
    {
        // 定义动画的总时长
        float totalDuration = zoomDuration;

        // 同步缩放和平移动画
        DOTween.To(() => faceMaterial.GetVector("_UVScaleScale"), x => faceMaterial.SetVector("_UVScaleScale", x), (Vector4)targetScale, totalDuration*0.5f).SetEase(Ease.OutCubic);
        DOTween.To(() => faceMaterial.GetVector("_UVScalePivot"), x => faceMaterial.SetVector("_UVScalePivot", x), (Vector4)targetPivot, totalDuration).SetEase(Ease.OutCubic).OnComplete(onComplete);
    }

    private void FadeToBlack(TweenCallback onComplete)
    {
        blackScreenImage.gameObject.SetActive(true);
        blackScreenImage.DOFade(1f, 1f).OnComplete(onComplete);
    }

    private void FadeFromBlack(TweenCallback onComplete)
    {
        blackScreenImage.DOFade(0f, 1f).OnComplete(() =>
        {
            blackScreenImage.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    private void ActivateEyeView()
    {
        eyePanel.SetActive(true);
        currentViewState = ViewState.EyeView;
        FadeFromBlack(() => isTransitioning = false);
    }
}

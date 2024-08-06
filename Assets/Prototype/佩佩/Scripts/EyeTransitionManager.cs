using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EyeTransitionManager : MonoBehaviour
{
    public float zoomDuration = 1f;
    public Image blackScreenImage;
    public RectTransform faceRectTransform;

    [System.Serializable]
    public class ViewConfig
    {
        public GameObject viewPanel;
        public Button activateButton; // 视图的激活按钮（MainView除外）
        public Button returnToMainViewButton; // 视图的返回MainView按钮
        public Vector2 viewScale;
        public Vector2 viewPosition;
        public ViewState viewState;
    }

    [SerializeField]
    private List<ViewConfig> viewConfigs; // 在 Unity 编辑器中配置视图（不包括MainView）

    private Dictionary<ViewState, ViewConfig> viewConfigDictionary;
    private bool isTransitioning = false;

    public enum ViewState
    {
        MainView,
        EyeView,
        UFView,
        MemoryView
        // 为未来的视图添加更多状态
    }

    private ViewState currentViewState = ViewState.MainView;

    private void Start()
    {
        InitializeView();

        viewConfigDictionary = new Dictionary<ViewState, ViewConfig>();

        foreach (var config in viewConfigs)
        {
            viewConfigDictionary[config.viewState] = config;
            if (config.activateButton != null)
            {
                // 绑定非MainView的视图激活按钮
                config.activateButton.onClick.AddListener(() => StartTransition(config.viewState));
            }
            if (config.returnToMainViewButton != null)
            {
                // 绑定每个视图的返回MainView按钮
                config.returnToMainViewButton.onClick.AddListener(() => StartTransition(ViewState.MainView));
            }
        }
    }

    private void InitializeView()
    {
        // 默认激活MainView
        SetView(ViewState.MainView);
    }

    private void SetView(ViewState viewState)
    {
        // 如果是MainView，就设置为默认值
        if (viewState == ViewState.MainView)
        {
            faceRectTransform.localScale = Vector2.one; // 或者MainView的默认缩放
            faceRectTransform.anchoredPosition = Vector2.zero; // 或者MainView的默认位置
            foreach (var viewConfig in viewConfigs)
            {
                viewConfig.viewPanel.SetActive(false);
            }
            return;
        }

        if (!viewConfigDictionary.TryGetValue(viewState, out ViewConfig config)) return;

        faceRectTransform.localScale = config.viewScale;
        faceRectTransform.anchoredPosition = config.viewPosition;

        foreach (var viewConfig in viewConfigs)
        {
            viewConfig.viewPanel.SetActive(viewConfig.viewState == viewState);
        }
    }

    private void StartTransition(ViewState targetViewState)
    {
        if (isTransitioning || currentViewState == targetViewState) return;
        isTransitioning = true;

        if (targetViewState == ViewState.MainView)
        {
            // Zoom Out 的时候，先黑屏淡入
            blackScreenImage.gameObject.SetActive(true);
            Sequence zoomOutSequence = DOTween.Sequence();
            zoomOutSequence.Append(blackScreenImage.DOFade(1f, zoomDuration / 2));

            // 在黑屏淡入完成后关闭 Eye Panel
            zoomOutSequence.AppendCallback(() =>
            {
                foreach (var viewConfig in viewConfigs)
                {
                    viewConfig.viewPanel.SetActive(false);
                }
            });

            // 开始淡出黑屏
            zoomOutSequence.Append(blackScreenImage.DOFade(0f, zoomDuration).OnComplete(() => blackScreenImage.gameObject.SetActive(false)));

            // 在淡出黑屏到一半时开始 Zoom Out
            zoomOutSequence.Insert(zoomDuration / 2, faceRectTransform.DOAnchorPos(Vector2.zero, zoomDuration).SetEase(Ease.OutCubic));
            zoomOutSequence.Insert(zoomDuration / 2, faceRectTransform.DOScale(Vector2.one, zoomDuration).SetEase(Ease.OutCubic));

            // 在整个动画完成后更新视图状态
            zoomOutSequence.OnComplete(() =>
            {
                currentViewState = ViewState.MainView;
                isTransitioning = false;
            });
        }
        else
        {
            if (!viewConfigDictionary.TryGetValue(targetViewState, out ViewConfig targetConfig)) return;

            // Zoom In 的时候，开始 Zoom
            Sequence zoomInSequence = DOTween.Sequence();
            zoomInSequence.Append(faceRectTransform.DOAnchorPos(targetConfig.viewPosition, zoomDuration).SetEase(Ease.OutCubic));
            zoomInSequence.Join(faceRectTransform.DOScale(targetConfig.viewScale, zoomDuration).SetEase(Ease.OutCubic));

            // 在 Zoom 到一半时开始黑屏过渡
            zoomInSequence.Insert(zoomDuration / 2, blackScreenImage.DOFade(1f, zoomDuration / 2).OnStart(() => blackScreenImage.gameObject.SetActive(true)));

            // 在整个 Zoom 完成后激活 Eye View
            zoomInSequence.OnComplete(() =>
            {
                SetView(targetViewState);
                FadeFromBlack(() =>
                {
                    currentViewState = targetViewState;
                    isTransitioning = false;
                });
            });
        }
    }

    private void FadeFromBlack(TweenCallback onComplete)
    {
        blackScreenImage.DOFade(0f, 1f).OnComplete(() =>
        {
            blackScreenImage.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}

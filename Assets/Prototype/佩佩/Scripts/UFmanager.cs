using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UFmanager : MonoBehaviour
{
    public Image photoImage; // 用于显示"相片"的UI Image组件
    public Image baseImage; // 用于显示底板的UI Image组件

    public Transform startTransform; // 照片开始的位置
    public Transform endTransform; // 照片结束的位置

    private bool isShowingPhoto = false; // 是否正在显示照片的标志

    // "Draw" 按钮的点击事件处理
    public void OnDrawButtonPressed(Sprite photo)
    {
        if (!isShowingPhoto)
        {
            ShowPhoto(photo);
        }
    }

    public void ShowPhoto(Sprite photo)
    {
        isShowingPhoto = true; // 设置标志，表示照片显示正在进行

        // 激活底板
        baseImage.gameObject.SetActive(true);
        // 设置照片并使其可见
        photoImage.sprite = photo;
        photoImage.gameObject.SetActive(true);

        // 初始化透明度为 0
        Color c = photoImage.color;
        c.a = 0f;
        photoImage.color = c;

        // 设置照片初始位置
        baseImage.rectTransform.position = startTransform.position;

        // 移动和淡入照片
        float moveAndFadeInTime = 4.0f;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(baseImage.rectTransform.DOMove(endTransform.position, moveAndFadeInTime));
        mySequence.Join(photoImage.DOFade(1f, moveAndFadeInTime).SetEase(Ease.InOutQuad));
        mySequence.OnComplete(() => {
            isShowingPhoto = false; // 动画完成后，重置标志
        });
    }

    // ...其他方法和逻辑
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShootingManager : MonoBehaviour
{
    public GameObject photoScene; // 包含前景，中景，背景的父物体
    public Slider focusSlider; // 焦点滑块
    public Camera mainCamera; // 主相机
    public Camera shootingCamera; // 拍照相机
    public Image photoImage; // 用于显示"相片"的UI Image组件
    public Image baseImage; // 用于显示底板的UI Image组件

    private int forwardFocus = 2;
    private int midFocus = 4;
    private int backFocus = 8;
    private int currentFocus = -1; // 当前焦点

    public Camera viewCamera;
    public RawImage viewImage;

    public Transform startTransform; // 照片开始的位置
    public Transform endTransform; // 照片结束的位置


    private void Start()
    {
        ShowView();
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        shootingCamera.targetTexture = renderTexture;
        // 设置滑块的范围和步长
        focusSlider.minValue = 0;
        focusSlider.maxValue = 8;
        focusSlider.wholeNumbers = true;

        // 当滑块的值改变时，更新焦点
        focusSlider.onValueChanged.AddListener(UpdateFocus);
    }

    // 更新焦点
    private void UpdateFocus(float value)
    {
        currentFocus = (int)value;
    }
    private void ShowView()
    {
         // Create a new RenderTexture
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        renderTexture.Create();

        // Set the camera to render to the RenderTexture
        viewCamera.targetTexture = renderTexture;

        // Set the RenderTexture as the texture for the RawImage
        viewImage.texture = renderTexture;

    }

    // 查找所有Sprite并添加到列表中
  private void FindSpritesAndAddToList(Transform parent, List<_2dxFX_BlurHQX> list)
{
    _2dxFX_BlurHQX[] sprites = parent.GetComponentsInChildren<_2dxFX_BlurHQX>();
    foreach (_2dxFX_BlurHQX sprite in sprites)
    {
        list.Add(sprite);
    }
}

    // 应用模糊效果
    private void ApplyBlur(List<_2dxFX_BlurHQX> sprites, int focusPoint, string layerName)
    {
        float blurValue;
        foreach (_2dxFX_BlurHQX sprite in sprites)
        {
            blurValue = Mathf.Clamp01(Mathf.Abs(currentFocus - focusPoint) / 8f) * 4f;
            sprite.Blur = blurValue;
            //Debug.Log($"{layerName} blur: {blurValue}");
        }
    }
      private void DisApplyBlur(List<_2dxFX_BlurHQX> sprites)
    {
        foreach (_2dxFX_BlurHQX sprite in sprites)
        {
            sprite.Blur = 0;
        }
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }
    
        obj.layer = newLayer;
    
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // 拍照
    public IEnumerator TakePhotoProcess()
    {
        
        // 创建一个场景的克隆
        GameObject clone = Instantiate(photoScene);

        // 将克隆和拍照相机移动到一个新的层上
        clone.layer = 20; 
        SetLayerRecursively(clone, 20);
        shootingCamera.gameObject.layer = 20;

        // 让拍照相机只渲染层 20
        shootingCamera.cullingMask = 1 << 20;

        // 查找克隆中的所有Sprite并添加到列表中
        List<_2dxFX_BlurHQX> forwardSprites = new List<_2dxFX_BlurHQX>();
        List<_2dxFX_BlurHQX> midSprites = new List<_2dxFX_BlurHQX>();
        List<_2dxFX_BlurHQX> backSprites = new List<_2dxFX_BlurHQX>();

        FindSpritesAndAddToList(clone.transform.Find("Forward"), forwardSprites);
        FindSpritesAndAddToList(clone.transform.Find("Mid"), midSprites);
        FindSpritesAndAddToList(clone.transform.Find("Back"), backSprites);

        // 创建一个新的RenderTexture
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        shootingCamera.targetTexture = renderTexture;

        // 应用模糊效果并渲染一帧
        ApplyBlur(forwardSprites, forwardFocus, "Forward");
        ApplyBlur(midSprites, midFocus, "Mid");
        ApplyBlur(backSprites, backFocus, "Back");

        yield return new WaitForSeconds(0.2f);
        shootingCamera.Render();
        // 将RenderTexture中的图像转换为Texture2D
        Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photo.Apply();

        // 使用Texture2D来创建一个新的Sprite
        Sprite photoSprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));

        // 删除克隆并将相机的targetTexture设置为null
        Destroy(clone);
        shootingCamera.targetTexture = null;

        // 释放RenderTexture资源
        RenderTexture.active = null;
        renderTexture.Release();

        // 显示"相片"
        ShowPhoto(photoSprite);
        yield return null;
    }

    // 拍照
    public void TakePhoto()
    {
        // 开始拍照过程的协程
        StartCoroutine(TakePhotoProcess());
    }
    // 显示"相片"
    private void ShowPhoto(Sprite photo)
    {
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
        float moveAndFadeInTime = 4.0f; // 移动和淡入时间，可以根据需要调整
        baseImage.rectTransform.DOMove(endTransform.position, moveAndFadeInTime);
        photoImage.DOFade(1f, moveAndFadeInTime).SetEase(Ease.InOutQuad); // 使用Ease.InOutQuad曲线，可以根据需要调整
    }


    private void PrintDirectChildren(Transform parent)
    {
        Debug.Log("Direct children of " + parent.name + ":");
        foreach (Transform child in parent)
        {
            Debug.Log(child.name);
        }
    }

    
}

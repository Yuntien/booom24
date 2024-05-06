using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Window : MonoBehaviour
{
    // Start is called before the first frame updatepublic GameObject targetImage;  // 你的图像对象
    public float duration = 1.0f;  // 动画的持续时间
    public GameObject targetImage; 
    private Vector3 initialPosition =new Vector3(1.13f, -0.6f, 0);
    private Vector3 finalPosition = new Vector3(0, 0, 0);
    private Vector3 initialScale= new Vector3(1.7f, 1.7f, 0);
    private Vector3 finalScale = new Vector3(1.4f, 1.4f, 1.0f);

    // 这个方法应该在你想要开始动画的时候被调用
    private void Start() {
        targetImage.transform.position=initialPosition;
        targetImage.transform.localScale=initialScale;
        StartAnimation();
    }
    public void StartAnimation()
    {
        targetImage.SetActive(true);

        // Create a sequence
        Sequence sequence = DOTween.Sequence();

        // Add a delay at the beginning of the sequence
        sequence.AppendInterval(2);

        // Add the move and scale animations to the sequence
        sequence.Append(targetImage.transform.DOMove(finalPosition, duration));
        sequence.Join(targetImage.transform.DOScale(finalScale, duration));

       // StartCoroutine(AnimateImage());
    }


    /*private IEnumerator AnimateImage()
    {
        float elapsed = 0.0f;

        // 激活图像
        targetImage.SetActive(true);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // 使用 Vector3.Lerp() 在初始和最终位置、大小之间插值
            targetImage.transform.position = Vector3.Lerp(initialPosition, finalPosition, t);
            targetImage.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 确保在动画结束时图像的位置和大小是正确的
        targetImage.transform.position = finalPosition;
        targetImage.transform.localScale = finalScale;
    }*/
}

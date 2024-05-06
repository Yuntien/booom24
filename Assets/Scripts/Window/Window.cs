using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Window : Singleton<Window>
{
    // Start is called before the first frame updatepublic GameObject targetImage;  // 你的图像对象
    public float duration = 1.0f;  // 动画的持续时间

    public float Ropeduration = 1f; 
    public GameObject targetImage; 
    private Vector3 initialPosition =new Vector3(0.97f, -8.57f, 0);
    private Vector3 finalPosition = new Vector3(0, -6.71f, 0);
    private Vector3 initialScale= new Vector3(1.302303f, 1.302303f, 1.302303f);
    private Vector3 finalScale = new Vector3(1.129644f, 1.129644f, 1.129644f);
    private Vector3 ropeFinalPos=new Vector3(7.67f,4f,0);
    private Sequence lightSequence;

    public GameObject[] lights; // 将你的灯光拖到这个数组中
    public float lightDuration = 0.5f;
    public Transform rope;
    public Transform sender;
    public Transform policeCar;

    // 这个方法应该在你想要开始动画的时候被调用
    private void Start() {
        targetImage.transform.position=initialPosition;
        targetImage.transform.localScale=initialScale;
        //StartAnimation("普通");
    }
    public void StartAnimation(string animType)
    {
        targetImage.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        switch (animType)
    {
        case "普通":
            // 添加普通的动画
            sequence.AppendInterval(5);
            break;
        case "快递员":
            // 添加快递员的动画
            sequence.Append(sender.DOMove(new Vector3(13.66f,-4.370073f,0), 6f).SetEase(Ease.Linear));
            sequence.AppendCallback(() => Destroy(sender.gameObject));
            sequence.AppendInterval(2);

            break;
        case "警车":

            sequence.Append(policeCar.DOMove(new Vector3(7.12f,-4.135658f,0), 6f).SetEase(Ease.Linear));
            sequence.AppendCallback(() => Destroy(policeCar.gameObject));
            sequence.AppendInterval(2);
            break;
        case "销售大赛":
                sequence.AppendInterval(6);
                // 创建一个新的Sequence
                lightSequence = DOTween.Sequence();

                // 第一个循环，将每个灯光的激活添加到Sequence中
                foreach (var light in lights)
                {
                    // 假设你的灯光是通过改变GameObject的透明度来表示激活和关闭的
                    var spriteRenderer = light.GetComponent<SpriteRenderer>();

                    lightSequence.Append(spriteRenderer.DOFade(1, lightDuration)); // 激活灯光
                }

                // 第二个循环，将每个灯光的关闭添加到Sequence中
                foreach (var light in lights)
                {
                    // 假设你的灯光是通过改变GameObject的透明度来表示激活和关闭的
                    var spriteRenderer = light.GetComponent<SpriteRenderer>();

                    lightSequence.Append(spriteRenderer.DOFade(0, lightDuration)); // 关闭灯光
                }

                // 设置Sequence为循环
                lightSequence.SetLoops(-1);

                // 开始播放Sequence
                lightSequence.Play();

            // 添加销售带赛的动画
            //sequence.Append(targetImage.transform.DOMove(salesFinalPosition, duration));
            break;
        case "展品":
            sequence.AppendInterval(6);
            // 添加展品的动画
            //sequence.Append(targetImage.transform.DOMove(exhibitFinalPosition, duration));
            break;
        case "生小孩":
            sequence.AppendInterval(6);
            // 添加生小孩的动画
            //sequence.Append(targetImage.transform.DOMove(babyFinalPosition, duration));
            break;
        default:
            sequence.AppendInterval(6);
            // 如果 animType 的值不是以上任何一个，那么就添加一个默认的动画
            //sequence.Append(targetImage.transform.DOMove(defaultFinalPosition, duration));
            break;
    }
             
       //新增的表现
        sequence.Append(targetImage.transform.DOMove(finalPosition, duration));
        sequence.Join(targetImage.transform.DOScale(finalScale, duration));
        sequence.Append(rope.DOMove(ropeFinalPos, Ropeduration).SetEase(Ease.OutElastic));
    }



}

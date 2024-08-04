using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EyeManager : MonoBehaviour
{

    private List<EyeTarget> targets = new List<EyeTarget>(); // 改为EyeTarget类型

    public ViewCameraManager cameraManager; // 相机管理器
    public List<EyeTarget> Targets => targets; // 公开的Targets属性，也改为EyeTarget类型


    private EyeTarget currentTarget; // 改为EyeTarget类型
    public EyeTarget CurrentTarget => currentTarget; // 新增的公开 getter，也改为EyeTarget类型

    [SerializeField]
    private float colorTransitionDuration = 2f;

    private void Awake()
    {
        // 查找所有带有EyeTarget标签的物体
        GameObject[] eyeTargets = GameObject.FindGameObjectsWithTag("EyeTarget");
        foreach (GameObject go in eyeTargets)
        {
            EyeTarget eyeTarget = go.GetComponent<EyeTarget>();
            if (eyeTarget != null)
            {
                targets.Add(eyeTarget);
            }
        }

        // 设置初始目标的状态，无过渡效果
        
    }
    private void Start() {
        if (targets.Count > 0)
        {
            SetInitialTargetState(targets[0]);
        }
        
    }
    public void SetInitialTargetState(EyeTarget target)
    {
        // 直接设置目标为无色，无过渡效果
        target.SetToMaxBurnRadius();
        target.SetToMinBlur();
        cameraManager.SetCameraPosition(target.targetTransform);
        currentTarget=target;
    }


    public void SetTarget(EyeTarget newTarget)
    {
        StartCoroutine(SetTargetCoroutine(newTarget));
    }

    private IEnumerator SetTargetCoroutine(EyeTarget newTarget)
    {
        if (currentTarget != null)
        {

            currentTarget.FadeOut(colorTransitionDuration/3);
            yield return new WaitForSeconds(1f);
            cameraManager.MoveCameraTo(newTarget.targetTransform, colorTransitionDuration/3);
            // 等待FadeOut完成
            yield return new WaitForSeconds(colorTransitionDuration/3);
        }

        // 不要忘记更新 currentTarget 变量
        currentTarget = newTarget;

        // 这里需要检查 newTarget 是否为 null
        if (newTarget != null)
        {
            newTarget.ColorIn(colorTransitionDuration);
        }
    }
}

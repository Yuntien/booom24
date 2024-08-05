using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EyeManager : MonoBehaviour
{
    [SerializeField]
    private List<EyeTarget> targets = new List<EyeTarget>(); // 保存目标列表

    public List<EyeTarget> Targets => targets; // 提供目标列表的公共访问

    private EyeTarget currentTarget; // 当前选中的目标

    public EyeTarget CurrentTarget => currentTarget; // 提供当前目标的公共访问
    
    public enum EyeState
    {
        CanImagineColor,
        CannotImagineColor,
        EyeDisorder
    }
    public ViewCameraManager cameraManager;

    private IEyeStateEffect eyeStateEffect;

    private float colorTransitionDuration;
    
    [SerializeField]
    private EyeState currentState=EyeState.CanImagineColor;
        public EyeState CurrentState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                SetEyeState(value);
                currentState = value;
            }
        }
    }
    


    private void Awake()
    {
        InitializeTargets();

    }
    private void Start() 
    {
        if (targets.Count > 0)
        {
            SetInitialTargetState(targets[0]);
        }
        SetEyeState(currentState);
        
    }
    public void SetInitialTargetState(EyeTarget target)
    {
        currentTarget = target;
    }

    private void InitializeTargets()
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
    }
    public void SetTarget(EyeTarget newTarget)
    {
        if (newTarget != currentTarget)
        {
            StartCoroutine(SetTargetCoroutine(newTarget));
        }
    }
     private IEnumerator SetTargetCoroutine(EyeTarget newTarget)
    {
        float duration = eyeStateEffect.TransitionDuration;

        if (currentTarget != null)
        {
             eyeStateEffect?.ApplyUnfocusedEffect(currentTarget);
            yield return new WaitForSeconds(duration);
        }

        cameraManager.MoveCameraTo(newTarget.targetTransform, 1f);
        yield return new WaitForSeconds(1f);

        currentTarget = newTarget;

        if (newTarget != null)
        {
             eyeStateEffect?.ApplyFocusedEffect(newTarget);
            yield return new WaitForSeconds(duration);
        }
    }

    public void SetEyeState(EyeState state)
    {
        // Cleanup current state effect
        eyeStateEffect?.CleanupEffect(currentTarget);

        switch (state)
        {
            case EyeState.CanImagineColor:
                eyeStateEffect = new CanImagineColorEffect();
                break;
            case EyeState.CannotImagineColor:
                eyeStateEffect = new CannotImagineColorEffect();
                break;
            case EyeState.EyeDisorder:
                eyeStateEffect = new EyeDisorderEffect();
                break;
            default:
                throw new ArgumentException("Unknown eye state: " + state.ToString(), nameof(state));
        }

        // Update transition duration
        colorTransitionDuration = eyeStateEffect.TransitionDuration;

        // Initialize new state effect
        eyeStateEffect.InitializeEffect(currentTarget, cameraManager);
    }
}


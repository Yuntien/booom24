public interface IEyeStateEffect
{
    float TransitionDuration { get; }
    void InitializeEffect(EyeTarget target, ViewCameraManager cameraManager);
    void CleanupEffect(EyeTarget target);
    void ApplyFocusedEffect(EyeTarget target);
    void ApplyUnfocusedEffect(EyeTarget target);
}

public class CanImagineColorEffect : IEyeStateEffect
{
    public float TransitionDuration { get; } = 2f;

    public void InitializeEffect(EyeTarget target, ViewCameraManager cameraManager)
    {
        target.SetToMaxBurnRadius();
        target.SetToMinBlur();
        cameraManager.SetCameraPosition(target.targetTransform);
    }

    public void CleanupEffect(EyeTarget target)
    {
        target.SetToMinBurnRadius();
        target.SetToMaxBlur();
        // Add cleanup logic here
    }

    public void ApplyFocusedEffect(EyeTarget target)
    {
        target.ColorIn(TransitionDuration);
    }

    public void ApplyUnfocusedEffect(EyeTarget target)
    {
        target.FadeOut(TransitionDuration);
    }
}

public class CannotImagineColorEffect : IEyeStateEffect
{
    public float TransitionDuration { get; } = 1f;

    public void InitializeEffect(EyeTarget target, ViewCameraManager cameraManager)
    {
        cameraManager.SetCameraPosition(target.targetTransform);
        target.SetToMinBlur(false);
    }

    public void CleanupEffect(EyeTarget target)
    {
        // Add cleanup logic here
        target.SetToMaxBlur(false);
    }

    public void ApplyFocusedEffect(EyeTarget target)
    {
        target.BlurOut(TransitionDuration);
    }

    public void ApplyUnfocusedEffect(EyeTarget target)
    {
        target.BlurIn(TransitionDuration);
    }
}

public class EyeDisorderEffect : IEyeStateEffect
{
    public float TransitionDuration { get; } = 2f;

    public void InitializeEffect(EyeTarget target, ViewCameraManager cameraManager)
    {
        // Add initialization logic here
    }

    public void CleanupEffect(EyeTarget target)
    {
        // Add cleanup logic here
    }

    public void ApplyFocusedEffect(EyeTarget target)
    {
        target.ColorIn(TransitionDuration);
    }

    public void ApplyUnfocusedEffect(EyeTarget target)
    {
        target.FadeOut(TransitionDuration);
    }
}

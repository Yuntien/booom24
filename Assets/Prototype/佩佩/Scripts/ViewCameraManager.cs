using UnityEngine;
using DG.Tweening;

public class ViewCameraManager : MonoBehaviour
{
    public Transform targetTransform; // 目标位置
    public Vector2 minBounds; // 相机允许移动的最小边界
    public Vector2 maxBounds; // 相机允许移动的最大边界

    private Camera cam;
    private Vector3 smoothVelocity = Vector3.zero;
    public float smoothTime = 0.3f; // 相机移动的平滑时间

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    public void MoveCameraTo(Transform newTarget, float duration)
    {
        Vector3 targetPosition = newTarget.position;

        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;

        // 限制相机位置不超过边界，同时考虑到相机的大小
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + camWidth / 2, maxBounds.x - camWidth / 2);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + camHeight / 2, maxBounds.y - camHeight / 2);
        targetPosition.z = cam.transform.position.z; // 保持相机的Z轴位置不变

        // 使用 DOTween 平滑地移动相机
        cam.transform.DOMove(targetPosition, duration).SetEase(Ease.InOutSine);
    }
    public void SetCameraPosition(Transform newTarget)
    {
        Vector3 targetPosition = newTarget.position;

        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;

        // 限制相机位置不超过边界，同时考虑到相机的大小
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + camWidth / 2, maxBounds.x - camWidth / 2);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + camHeight / 2, maxBounds.y - camHeight / 2);
        targetPosition.z = cam.transform.position.z; // 保持相机的Z轴位置不变

        // 直接设置相机位置
        cam.transform.position = targetPosition;
    }

    /*private void LateUpdate()
    {
        if (targetTransform != null)
        {
            Vector3 targetPosition = targetTransform.position;
            Vector3 cameraPosition = cam.transform.position;

            float camHeight = cam.orthographicSize * 2;
            float camWidth = camHeight * cam.aspect;

            // 限制相机位置不超过边界，同时考虑到相机的大小
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + camWidth / 2, maxBounds.x - camWidth / 2);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + camHeight / 2, maxBounds.y - camHeight / 2);
            targetPosition.z = cameraPosition.z; // 保持相机的Z轴位置不变

            // 使用平滑阻尼来移动相机
            cam.transform.position = Vector3.SmoothDamp(cameraPosition, targetPosition, ref smoothVelocity, smoothTime);
        }
    }*/

    // 在编辑器中绘制相机边界
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(minBounds.x, minBounds.y), new Vector2(maxBounds.x, minBounds.y));
        Gizmos.DrawLine(new Vector2(maxBounds.x, minBounds.y), new Vector2(maxBounds.x, maxBounds.y));
        Gizmos.DrawLine(new Vector2(maxBounds.x, maxBounds.y), new Vector2(minBounds.x, maxBounds.y));
        Gizmos.DrawLine(new Vector2(minBounds.x, maxBounds.y), new Vector2(minBounds.x, minBounds.y));
    }
}

using System.Collections;
using UnityEngine;
using DG.Tweening;
public class CameraZoom : MonoBehaviour
{
    public Transform eyeTransform; // 眼睛的Transform
    public float zoomDuration = 1; // 缩放的持续时间

    public IEnumerator ZoomToEye()
    {
        // 计算缩放的目标位置和大小
        Vector3 targetPosition = eyeTransform.position;
        float targetSize = 0.1f; // 你可以根据需要调整这个值

        // 缩放相机
        Camera.main.transform.DOMove(targetPosition, zoomDuration);
        Camera.main.DOOrthoSize(targetSize, zoomDuration);

        // 等待缩放完成
        yield return new WaitForSeconds(zoomDuration);
    }
}

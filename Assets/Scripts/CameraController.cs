
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;//表明摄像头跟随目标
    [SerializeField] private Vector2 minMaxXY;//边界约束

  private void LateUpdate()
  {
        if (target == null)
        {
            Debug.LogWarning("未选中目标");

            return;
        }
    Vector3 targetPosition=target.position;//寻找目标位置
    targetPosition.z=-10;//将摄像头放到画面前方
        targetPosition.x = Mathf.Clamp(targetPosition.x, -minMaxXY.x, minMaxXY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -minMaxXY.y, minMaxXY.y);
        //利用 Mathf.Clamp 函数限制 targetPosition 在 X 和 Y 方向上的值，使其不超出 -minMaxXY.x 到 minMaxXY.x
        transform.position=targetPosition;
  }
}

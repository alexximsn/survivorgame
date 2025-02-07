
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Transform target;
    [SerializeField] private Vector2 minMaxXY;



  private void LateUpdate()
  {
        if (target == null)
        {
            Debug.LogWarning("未选中目标");

            return;
        }
    Vector3 targetPosition=target.position;
    targetPosition.z=-10;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -minMaxXY.x, minMaxXY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -minMaxXY.y, minMaxXY.y);
        transform.position=targetPosition;
  }
}

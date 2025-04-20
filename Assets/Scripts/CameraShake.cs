using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;

    private void Awake() => Gunweapon.onButtetShot += Shake;

    private void OnDestroy() => Gunweapon.onButtetShot -= Shake;

    private void Shake()
    {
        Vector2 direction = Random.onUnitSphere.With(z: 0).normalized;

        transform.localPosition = Vector3.zero;

        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, direction * shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake);
    }
}

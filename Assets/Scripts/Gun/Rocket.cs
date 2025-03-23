using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Rocket : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask wallMask;

    private new Rigidbody2D rigidbody;
    private Collider2D collider;
    private bool isReleased;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    public void ShootStraight(Vector2 direction)
    {
        isReleased = false;
        rigidbody.velocity = direction * speed;
        transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReleased) return;

        int layer = other.gameObject.layer;
        if (IsInLayerMask(layer, wallMask) || IsInLayerMask(layer, enemyMask))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isReleased) return;

        // 生成爆炸特效
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;
        exp.transform.rotation = Quaternion.identity;

        // 回收自身
        StartCoroutine(ReleaseWithDelay(0.3f));
    }

    private IEnumerator ReleaseWithDelay(float delay)
    {
        isReleased = true;
        rigidbody.velocity = Vector2.zero;
        collider.enabled = false;

        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.PushObject(gameObject);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    public void ResetState()
    {
        isReleased = false;
      //  arrived = false;
        collider.enabled = true;
        gameObject.SetActive(true);
    }
}

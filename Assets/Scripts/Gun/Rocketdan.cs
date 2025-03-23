using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketdan : Gunweapon
{
    [Header("Rocket Settings")]
    [SerializeField] public int rocketNum = 5;
    [SerializeField] public float rocketAngle = 30f;

    protected override void Shoot()
    {
        StartCoroutine(DelayFire(0.2f));
    }

    private IEnumerator DelayFire(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector2 shootDirection = (mousePos - (Vector2)transform.position).normalized;
        float angleStep = rocketAngle / (rocketNum - 1);

        for (int i = 0; i < rocketNum; i++)
        {
            GameObject rocketObj = ObjectPool.Instance.GetObject(bulletPrefab);
            rocketObj.transform.position = shootingPoint.position;

            // 霰弹枪式对称散布
            float angleOffset = -rocketAngle / 2 + angleStep * i;
            Vector2 spreadDir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * shootDirection;

            Rocket rocket = rocketObj.GetComponent<Rocket>();
            rocket.ShootStraight(spreadDir.normalized);
        }
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        base.UpdateStats(playerStatsManager);
        // 可添加火箭专属属性强化逻辑
        rocketNum += Mathf.RoundToInt(playerStatsManager.GetStatValue(Stat.SpreadCount));
    }

}

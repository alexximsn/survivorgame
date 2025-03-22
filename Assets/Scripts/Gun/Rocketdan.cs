using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketdan : Gunweapon
{
    //public int rocketNum = 3;
    //public float rocketAngle = 15;

    //protected override void Shoot()
    //{
    //    animator.SetTrigger("Shoot");
    //    StartCoroutine(DelayFire(.2f));
    //}

    //IEnumerator DelayFire(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    int median = rocketNum / 2;
    //    for (int i = 0; i < rocketNum; i++)
    //    {
    //        Bullet bullet = bulletPool.Get();
    //        bullet.transform.position = shootingPoint.position;

    //        Vector2 modifiedDirection;
    //        if (rocketNum % 2 == 1)
    //        {
    //            modifiedDirection = Quaternion.AngleAxis(rocketAngle * (i - median), Vector3.forward) * direction;
    //        }
    //        else
    //        {
    //            modifiedDirection = Quaternion.AngleAxis(rocketAngle * (i - median) + rocketAngle / 2, Vector3.forward) * direction;
    //        }

    //        if (bullet is RocketBullet rocket)
    //        {
    //            rocket.SetTarget(mousePos);
    //            rocket.Shoot(GetDamage(out bool isCritical), modifiedDirection, isCritical);
    //        }
    //    }
    //}

}

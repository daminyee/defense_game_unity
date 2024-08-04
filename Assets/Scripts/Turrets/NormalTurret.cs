using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalTurret : BaseTurret
{
    void Start()
    {
        this.attackPower = 1;
        this.attackSpeed = 0.5f;
    }

    void Update()
    {
        if (!this.isInstalledTurret) return;

        WatchEnemy();
        if (targetEnemy != null)
        {
            //Debug.Log("d");
            if (attackCoolCount > 2)
            {
                AttackEnemy();
                attackCoolCount = 0;
            }
        }
        attackCoolCount += Time.deltaTime * attackSpeed;

        DebugRay();
        DebugTarget();
    }

    public override void Attack()
    {
        var newBullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        var bullet = newBullet.GetComponent<Bullet>();
        bullet.Initialize(this.attackPower, this.transform.position, 0, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : BaseTurret
{
    void Start()
    {
        this.attackPower = 2f;
        this.attackSpeed = 0.13f;
    }

    void Update()
    {
        if (!this.isInstalledTurret) return;

        WatchEnemy();
        if (targetEnemy != null)
        {
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
        var bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<Laser>().attackDamage = this.attackPower;
    }
}

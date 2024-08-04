using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizardTurret : BaseTurret
{
    public float slowPower;
    void Start()
    {
        this.attackPower = 0.5f;
        this.attackSpeed = 0.13f;
        slowPower = 0.5f;
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
    }

    public override void Attack()
    {
        var newBullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        var bullet = newBullet.GetComponent<IceBullet>();
        bullet.Initialize(this.attackPower, this.transform.position, this.slowPower);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizardTurret : BaseTurret
{
    private float slowPower;
    void Start()
    {
        this.attackPower = 0.5f;
        this.attackSpeed = 0.5f;
        slowPower = 0.3f;
    }

    void Update()
    {
        if(!this.isInstalledTurret) return;

        WatchEnemy();
        if(targetEnemy != null)
        {
            if(attackCoolCount > 2)
            {
                AttackEnemy();
                attackCoolCount = 0;
            }
        }
        attackCoolCount += Time.deltaTime * attackSpeed;
    }

    public override void Attack()
    {
        var bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<Bullet>().attackDamage = this.attackPower;
        bullet.GetComponent<Bullet>().slowPower = this.slowPower;
        // bullet.GetComponent<Bullet>().isSlowBullet = true;
    }

    public override void UpgradeTurret()
    {
        this.attackPower += 0.5f;
        this.attackSpeed += 0.2f;
        this.slowPower += 0.1f;
    }
}

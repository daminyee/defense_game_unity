using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalTurret : BaseTurret
{

    void Start()
    {
        this.Initialize();
        this.attackPower = 1;
        this.attackSpeed = 0.5f;
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
    }

    public override void UpgradeTurret()
    {
        this.attackPower += 1;
        this.attackSpeed += 0.5f;
    }
}

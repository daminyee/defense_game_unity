using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTurret : BaseTurret
{
    public GameObject leftBody;
    public GameObject rightBody;
    void Start()
    {
        this.attackPower = 0.7f;
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

        var leftBullet = Instantiate(bulletPrefab, this.leftBody.transform.position, this.transform.rotation);
        leftBullet.GetComponent<Bullet>().attackDamage = this.attackPower/3;
        var rightBullet = Instantiate(bulletPrefab, this.rightBody.transform.position, this.transform.rotation);
        rightBullet.GetComponent<Bullet>().attackDamage = this.attackPower/3;
    }

    public override void UpgradeTurret()
    {
        this.attackPower += 0.5f;
        this.attackSpeed += 0.5f;
    }
}

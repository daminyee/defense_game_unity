using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusTurret : BaseTurret
{
    Quaternion attackRotation;
    // Start is called before the first frame update
    void Start()
    {
        this.attackPower = 1;
        this.attackSpeed = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.isInstalledTurret) return;
        
        WatchEnemy();
        if(targetEnemy != null)
        {
            if(attackCoolCount > 2)
            {
                attackRotation = this.transform.rotation;
                AttackEnemy();
                attackCoolCount = 0;
            }
        }
        attackCoolCount += Time.deltaTime * attackSpeed;
    }


    public override void Attack()
    {
        for (int i = 0; i <= 360; i += 45)
        {
            var turretRotation = this.transform.rotation;
            attackRotation = Quaternion.Euler(0, 0, turretRotation.eulerAngles.z + i);
            var newBullet = Instantiate(bulletPrefab, this.transform.position, attackRotation);
            var bullet = newBullet.GetComponent<Bullet>();
            bullet.Initialize(this.attackPower,this.transform.position, 0, false);
        }


    }
    // public override void UpgradeTurret()
    // {
    //     this.attackPower += 1;
    //     this.attackSpeed += 0.5f;
    // }
}

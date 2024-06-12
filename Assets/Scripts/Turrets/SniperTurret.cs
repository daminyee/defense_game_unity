using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTurret : BaseTurret
{
    void Start()
    {
        this.attackPower = 2;
        this.attackSpeed = 0.3f;
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
        var newBullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        var bullet = newBullet.GetComponent<SniperBullet>();
        bullet.Initialize(this.attackPower, this.transform.position);
    }

    // public void UpgradeTurret()
    // {
    //     this.attackPower += 1.7f;
    //     this.attackSpeed += 0.1f;
    //     upgradeCount += 1;
    //     // if(upgradeCount >= 3)
    //     // {
    //     //     GameObject newTurretGameObject = Instantiate(sniperTurretPrefab, this.transform.position, Quaternion.identity);
    //     //     //LayerTurret laserTurret= newTurretGameObject.GetComponent<LaserTurret>();

    //     //     turretSpace.installedTurret = laserTurret;
    //     //     laserTurret.turretSpace = this.turretSpace;
    //     //     laserTurret.canvasUI = this.canvasUI;
    //     //     laserTurret.mainCamera = this.mainCamera;
    //     //     laserTurret.isInstalledTurret = true;
    //     //     laserTurret.MakeAttackRangeInvisible();

    //     //     Destroy(this.gameObject);
    //     // }
    // }
}

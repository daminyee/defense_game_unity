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
        var bullet = newBullet.GetComponent<Bullet>();
        bullet.Initialize(this.attackPower, this.transform.position, 0, false);

        var newLeftBullet = Instantiate(bulletPrefab, this.leftBody.transform.position, this.transform.rotation);
        var leftBullet = newLeftBullet.GetComponent<Bullet>();
        leftBullet.Initialize(this.attackPower / 3, this.transform.position, 0, false);

        var newRightBullet = Instantiate(bulletPrefab, this.rightBody.transform.position, this.transform.rotation);
        var rightbullet = newRightBullet.GetComponent<Bullet>();
        bullet.Initialize(this.attackPower / 3, this.transform.position, 0, false);
    }

    // public override void UpgradeTurret()
    // {
    //     this.attackPower += 0.5f;
    //     this.attackSpeed += 0.5f;
    // }
}

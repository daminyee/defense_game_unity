using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalTurret : BaseTurret
{
    int upgradeCount;
    public GameObject tripleTurretPrefab;
    void Start()
    {
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
        
        // var range = this.attackRange.GetComponent<CircleCollider2D>().bounds.size;
        
        // var ray = new Ray(this.transform.position, this.transform.right);
        // var hit = Physics2D.RaycastAll(ray.origin, ray.direction, range.x/2);
        // // // show ray
        // Debug.DrawRay(ray.origin, ray.direction * range.x/2, Color.red);
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
        upgradeCount += 1;
        if(upgradeCount >= 3)
        {
            GameObject newTurretGameObject = Instantiate(tripleTurretPrefab, this.transform.position, Quaternion.identity);
            BlizardTurret tripleTurret= newTurretGameObject.GetComponent<BlizardTurret>();

            turretSpace.installedTurret = tripleTurret;
            tripleTurret.turretSpace = this.turretSpace;
            tripleTurret.canvasUI = this.canvasUI;
            tripleTurret.mainCamera = this.mainCamera;
            tripleTurret.isInstalledTurret = true;
            tripleTurret.MakeAttackRangeInvisible();

            Destroy(this.gameObject);
        }
    }
    // public override void SellTurret()
    // {
    // }
}

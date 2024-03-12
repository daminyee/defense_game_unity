using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3월 12일까지 숙제
// four direction turret class 완성하기
// 8방향까지 updgrade 해보기
// 느리게하는 터렛도 upgrade turret새로 만들어보기
public class FourDirectionTurret : BaseTurret
{
    int upgradeCount;

    Quaternion attackRotation;
    public GameObject cactusTurretPrefab;
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
        for (int i = 0; i <= 360; i += 90)
        {
            var turretRotation = this.transform.rotation;
            attackRotation = Quaternion.Euler(0, 0, turretRotation.eulerAngles.z + i);
            var bullet = Instantiate(bulletPrefab, this.transform.position, attackRotation);
            bullet.GetComponent<Bullet>().attackDamage = this.attackPower;
        }


    }
    public override void UpgradeTurret()
    {
        this.attackPower += 1;
        this.attackSpeed += 0.5f;
        upgradeCount += 1;

        if(upgradeCount >= 3)
        {
            GameObject newTurretGameObject = Instantiate(cactusTurretPrefab, this.transform.position, Quaternion.identity);
            CactusTurret cactusTurret= newTurretGameObject.GetComponent<CactusTurret>();

            turretSpace.installedTurret = cactusTurret;
            cactusTurret.turretSpace = this.turretSpace;
            cactusTurret.canvasUI = this.canvasUI;
            cactusTurret.mainCamera = this.mainCamera;
            cactusTurret.isInstalledTurret = true;
            cactusTurret.MakeAttackRangeInvisible();

            Destroy(this.gameObject);
        }
    }
}

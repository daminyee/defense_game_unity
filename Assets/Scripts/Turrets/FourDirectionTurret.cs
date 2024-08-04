using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3월 12일까지 숙제
// four direction turret class 완성하기
// 8방향까지 updgrade 해보기
// 느리게하는 터렛도 upgrade turret새로 만들어보기
public class FourDirectionTurret : BaseTurret
{
    Quaternion attackRotation;

    void Start()
    {
        this.attackPower = 1;
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
                attackRotation = this.transform.rotation;
                //AttackEnemy();
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
            var newBullet = Instantiate(bulletPrefab, this.transform.position, attackRotation);
            var bullet = newBullet.GetComponent<Bullet>();
            bullet.Initialize(this.attackPower, this.transform.position, 0, false);
        }


    }
}

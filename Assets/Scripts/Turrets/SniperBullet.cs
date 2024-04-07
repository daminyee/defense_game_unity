using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public float attackDamage;

    public Vector2 shootingOrigin;
    public bool shouldDestroyWhenOutOfRange;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(18 * Time.deltaTime, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            enemy.GotHit(attackDamage);
        }
    }
    void OnBecameInvisible() //화면밖으로 나가 보이지 않게 되면 호출이 된다.
    {
        Destroy(this.gameObject); //객체를 삭제한다.
    }

    public void Initialize(float attackDamage, Vector2 shootingOrigin, bool shouldDestroyWhenOutOfRange = true)
    {
        this.attackDamage = attackDamage;
        //this.attackRange = attackRange;
        //this.attackRangeRadius = attackRangeRadius;
        this.shootingOrigin = shootingOrigin;
        this.shouldDestroyWhenOutOfRange = shouldDestroyWhenOutOfRange;
    }
}

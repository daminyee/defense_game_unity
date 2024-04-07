using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3월 24일 과제
// 1. attackRange의 의존도를 bullet에서 없애기 (아래 코드 참고)
// 2. bullet이 attackRange를 통과하면 사라지게 하기
public class Bullet : MonoBehaviour
{
    public float attackDamage;
    public float slowPower;
    
    public bool isSlowBullet;

    //public GameObject attackRange;
    //public GameObject attackRange_CircleCollider;
    
    public float attackRangeRadius;

    public Vector2 shootingOrigin;

    public bool shouldDestroyWhenOutOfRange = true;

    public void Initialize(float attackDamage, Vector2 shootingOrigin, float slowPower, bool isSlowBullet, bool shouldDestroyWhenOutOfRange = true)
    {
        this.attackDamage = attackDamage;
        //this.attackRange = attackRange;
        //this.attackRangeRadius = attackRangeRadius;
        this.shootingOrigin = shootingOrigin;
        this.slowPower = slowPower;
        this.isSlowBullet = isSlowBullet;
        this.shouldDestroyWhenOutOfRange = shouldDestroyWhenOutOfRange;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 이거 완성하기
        transform.Translate(6 * Time.deltaTime, 0, 0);
        // attackRange에 대한 의존도를 없애라! (3월 20일 수업)
        // float attackRangeRadius = attackRange.radius;
        // float distance = Vector2.Distance(transform.position, shootingOrigin);
        // if(distance > attackRangeRadius && shouldDestroyWhenOutOfRange)
        // {
        //     Debug.Log(attackRangeRadius);
        //     //Destroy(this.gameObject);
        // }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        //var shield = collider.GetComponent<Shield>();

        // if(shield != null)
        // {
        //     Destroy(this.gameObject);
        // }
        if(enemy != null)
        {
            enemy.GotHit(attackDamage);
            if(isSlowBullet) enemy.SlowDown(slowPower);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var attackRange = collider.GetComponent<AttackRange>();
        if(attackRange != null)
        {
            Destroy(this.gameObject);
        }        
    }

    
    void OnBecameInvisible() //화면밖으로 나가 보이지 않게 되면 호출이 된다.
    {
        Destroy(this.gameObject); //객체를 삭제한다.
    }
}

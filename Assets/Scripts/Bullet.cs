using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackDamage;
    public float slowPower;
    
    public bool isSlowBullet;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(6 * Time.deltaTime, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();

        if(enemy != null)
        {
            enemy.GotHit(attackDamage);
            if(isSlowBullet) enemy.SlowDown(slowPower);
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible() //화면밖으로 나가 보이지 않게 되면 호출이 된다.
    {
        Destroy(this.gameObject); //객체를 삭제한다.
    }
}

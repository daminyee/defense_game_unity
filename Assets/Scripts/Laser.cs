using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float attackDamage;
    void Start()
    {
        attackDamage = 99;
        StartCoroutine(PlayDisappearAnimation());
        var childLaserObject = gameObject.transform.GetChild(0).gameObject;
        // add ontriggerenter2d to child laser object
        var childLaser = childLaserObject.AddComponent<BoxCollider2D>();
        childLaser.isTrigger = true;
        childLaser.size = new Vector2(0.1f, 0.1f);
        childLaser.offset = new Vector2(0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider);
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            //Debug.Log(enemy);
            enemy.GotHit(attackDamage);
        }
    }

    void OnBecameInvisible() //화면밖으로 나가 보이지 않게 되면 호출이 된다.
    {
        Destroy(this.gameObject); //객체를 삭제한다.
    }

    public IEnumerator PlayDisappearAnimation()
    {
        float time = 0;
        while (time < 2f)
        {
            time += Time.deltaTime;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color.a - 0.01f);
            //GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, GetComponent<SpriteRenderer>().color.a - 0.015f);
            yield return null; // while문 안에 yield return null을 넣어주는 이유는 while문이 돌면서 1프레임씩 기다려주기 위함
        }
        Destroy(gameObject);
    }
}

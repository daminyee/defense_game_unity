using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2024.03.12 까지
// 1. SlowField 완성하기 (=slow turret을 완성하기)
    // 1.1 몇 초뒤에 slowfield가 사라지게 하는 것까지
    // 1.2. 자유, 하지만 알아서 재밌게
// 2. SniperTurret 관통하는 친구 (얘가 제일 비싼 친구로 하기로)
    // 2.1 업그레이드 어떻게 할지 생각
    // 2.2 레이저 광선이 나가는 것도 약간 잘 보여주기

// 2024.03.19 까지
// 1. SlowField 완성하기 (=slow turret을 완성하기)
    // 1.1 몇 초뒤에 slowfield가 사라지게 하는 것까지
    // 1.2. 자유, 하지만 알아서 재밌게
// 2. SniperTurret 관통하는 친구 (얘가 제일 비싼 친구로 하기로)
    // 2.1 레이저 도입
// 3. bullet이 사거리 넘어서 없애지게끔해야 한다. (bullet이 사거리 넘어서 없애지게끔해야 한다.)
public class SlowField : MonoBehaviour
{
    
    public float slowPower;

    private float remainingTime = 2f;

    private bool isDestroying = false;

    public void Initialize(float slowPower, float attackDamage)
    {
        this.slowPower = slowPower;
    }

    void Start()
    {
        
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        if(remainingTime <= 0 && !isDestroying)
        {
            StartCoroutine(PlayDisappearAnimation());
            isDestroying = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            enemy.OnSlowField(this.slowPower);
            //enemy.OnSlowField(1f);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            enemy.OutSlowField();
        }
    }

    public IEnumerator PlayDisappearAnimation()
    {
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(140/255, 1, 127/5, gameObject.GetComponent<SpriteRenderer>().color.a - 0.02f);

            yield return null; // while문 안에 yield return null을 넣어주는 이유는 while문이 돌면서 1프레임씩 기다려주기 위함
        }
        Destroy(gameObject);
    }
}

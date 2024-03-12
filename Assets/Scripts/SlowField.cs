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
public class SlowField : MonoBehaviour
{
    
    public float slowPower;

    void Initialize(float slowPower)
    {
        this.slowPower = slowPower;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }




    void OnTriggerStay2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {

        }
    }
}

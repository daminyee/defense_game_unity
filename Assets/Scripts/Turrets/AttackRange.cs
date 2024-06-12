using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 적을 추적해야 하는데 고려해야할 주의사항
// 가장 중요한 고려사항: attackRange 안에서 제일 앞에 있는 적을 목표로 삼아야함
// Q. 어떻게 제일 앞에 있는지 판단할 것인가?
// A. 간격을 측정할 방법  
//  그러면 -> How? 1) Waypoint를 사용해서 측정 (나쁜 방법 아님, 복잡할 수 있겠다)
//       -> How? 2) AttackRange에서 각 적이 간 거리를 측정한다 (Update문에서) -> 문제: 분열하는 친구들이 문제다
//       -> How? 3) Waypoint순서를 바탕으로 Attackrange와 겹치는 끝점을 찾아서 그 끝점에 가장 가까운 적을 찾는다.

// 더 좋은 방법 -> 4) 각 enemy가 자기가 간 거리를 변수로 저장하게 하기, 그리고 turret을 공격을 할때 자기가 보고 있는 enemy 중 가장 먼 거리를 가진 enemy를 찾는다.

// 4/20일 오전 10시 숙제: 다민씨가 할 수 있는방법대로 한번 해보세요! (성공 안해도 돼요. 하지만, 본인이 이렇게 해봤다는 것을 저한테 설명을 해줘야된다)
//                이렇게 해야 앞으로 다른 문제를 해결할 때 좋은 연습이 될 것이다.

public class AttackRange : MonoBehaviour
{
    public float radius;

    //Vector2 meetingPoint;
    //BaseEnemy targetEnemy;

    void Start()
    {
        this.radius = 0.5f;
    }

    void Update()
    {
        // this.transform.localScale = new Vector3(radius, radius, 1);
        this.GetComponent<CircleCollider2D>().radius = radius;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            this.transform.parent.GetComponent<BaseTurret>().AddEnemy(enemy);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            this.transform.parent.GetComponent<BaseTurret>().DeleteEnemy(enemy);
        }
    }

    // void FindFrontLineEnemy() // meetingPoint = 사거리테두리와 경로가 만나는 지점
    // {

    //     foreach (BaseEnemy enemyToAttack in transform.parent.GetComponent<BaseTurret>().enemiesToAttack)
    //     {
    //         //Debug.Log(enemyToAttack + "is in queue");
    //         var distance = Vector2.Distance(enemyToAttack.transform.position, meetingPoint);
    //         if (targetEnemy == null)
    //         {
    //             targetEnemy = enemyToAttack;
    //         }
    //         else
    //         {
    //             var targetDistance = Vector2.Distance(targetEnemy.transform.position, meetingPoint);
    //             if (distance < targetDistance)
    //             {
    //                 targetEnemy = enemyToAttack;
    //             }
    //         }
    //     }
    // }
}

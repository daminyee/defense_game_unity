using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

// HW1
// Enemy 종류 3가지 만들기 (Enemy1, Enemy2, Enemy3)
// BaseEnemy에 pause 기능 (speed 0으로 만들기)
// 천천히 없애기 (StartCoroutine 사용) StartCoroutine은 뭔가 새로운 가지를 만들어서 따로 실행하는 친구
// BaseEnemy 쪽에 Destroy 기능 넣기 (Animation 투명도 0으로 만들기 - sprite alpha값을 0으로 만들기

public abstract class BaseEnemy : MonoBehaviour
{

    public WayPoint[] path;

    public GameObject head;
    public GameObject body;

    public int currentWayPointIndex = 0;
    private bool isMoving = false;

    public bool isSlowed = false;

    public float hp;

    public float originalSpeed = 1.0f;
    public float speed;

    public void Initialize()
    {
        this.InitializePosition();
        originalSpeed = speed;
    }

    public void InitializePosition() {
        this.currentWayPointIndex = 0;
        this.transform.position = this.path[this.currentWayPointIndex].transform.position;
    }

    public void MoveToNextWayPoint()
    {
        if (this.currentWayPointIndex < this.path.Length)
        {

            var targetPosition = this.path[this.currentWayPointIndex].transform.position;
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, this.speed * Time.deltaTime);

           // Get the direction to the target position
            Vector3 directionToTarget = targetPosition - transform.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Create a quaternion rotation around the Z-axis
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

            // Interpolate between current rotation and target rotation using Lerp
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            
            if (this.transform.position == this.path[this.currentWayPointIndex].transform.position)
            {
                this.currentWayPointIndex++;
                if(this.currentWayPointIndex == this.path.Length)
                {
                    var camera = GameObject.FindGameObjectWithTag("MainCamera");
                    var player = camera.GetComponent<Player>();

                    player.GotHit();
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void Pause()
    {
        this.speed = 0f;
    }

    public void GotHit(float attackDamage)
    {
        this.hp -= attackDamage;
        if(hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //Destroy(this.gameObject);
        StartCoroutine(FadeOut());
    }

    public void SlowDown(float slowPower)
    {
        if(!isSlowed)
        {
            this.speed -= slowPower;
            isSlowed = true;
            StartCoroutine(WaitThreeSecondsToReturnToOrignalSpeed());
        }
    }

    public void ReturnToOriginalSpeed()
    {
        this.speed = this.originalSpeed;
        isSlowed = false;
    }

    public IEnumerator WaitThreeSecondsToReturnToOrignalSpeed()
    {
        yield return new WaitForSeconds(3);
        ReturnToOriginalSpeed();
    }

    public IEnumerator FadeOut()
    {
        while (head.GetComponent<SpriteRenderer>().color.a > 0f && body.GetComponent<SpriteRenderer>().color.a > 0f)
        {
            head.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, head.GetComponent<SpriteRenderer>().color.a - 0.01f);
            body.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, body.GetComponent<SpriteRenderer>().color.a - 0.01f);
            if (head.GetComponent<SpriteRenderer>().color.a <= 0f && body.GetComponent<SpriteRenderer>().color.a <= 0f)
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}

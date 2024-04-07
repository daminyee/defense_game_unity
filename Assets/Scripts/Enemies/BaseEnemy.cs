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

    public Canvas canvasUI;
    public Camera mainCamera;


    public int currentWayPointIndex = 0;
    private bool isMoving = false;
    public bool isDie = false;

    public bool isSlowed = false;
    public bool isOnSlowField = false;

    public float hp;
    public int gold;

    public float originalSpeed = 1.0f;
    public float speed;

    public void Initialize(WayPoint[] path, Camera mainCamera, Canvas canvasUI, int currentWayPointIndex, Vector2 spawnPosition)
    {
        originalSpeed = speed;

        this.path = path;
        this.mainCamera = mainCamera;
        this.canvasUI = canvasUI;

        this.currentWayPointIndex = currentWayPointIndex;
        this.transform.position = spawnPosition;
    }

    public void MoveToNextWayPoint()
    {
        if(StaticValues.GetInstance().isPaused){
            return;
        }
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
        if(!isDie)
        {
            StaticValues.GetInstance().gold += this.gold;
            ShowGetGold(this.gold);
            //StartCoroutine(FadeOut());
        }
        isDie = true;
    }

    public void ShowGetGold(int getGold)
    {
        // create new text UI element
        // Debug.Log("showusedgold");
        GameObject getGoldText = new GameObject("UsedGoldText");
        getGoldText.transform.SetParent(canvasUI.transform, false);
        getGoldText.AddComponent<RectTransform>();
        getGoldText.AddComponent<CanvasRenderer>();
        getGoldText.AddComponent<UnityEngine.UI.Text>();
        getGoldText.GetComponent<UnityEngine.UI.Text>().text = "+" + getGold;
        getGoldText.GetComponent<UnityEngine.UI.Text>().color = Color.green;
        getGoldText.GetComponent<UnityEngine.UI.Text>().fontSize = 80;
        getGoldText.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleCenter;
        // change rect transform size
        getGoldText.GetComponent<RectTransform>().sizeDelta = new Vector2(230, 100);
        getGoldText.GetComponent<UnityEngine.UI.Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        // set position
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(this.transform.position);
        RectTransform rectTransform = getGoldText.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasUI.GetComponent<RectTransform>(), screenPoint, canvasUI.worldCamera, out localPoint);
        rectTransform.anchoredPosition = localPoint;

        // usedGoldText.GetComponent<RectTransform>().transform.position = textPos;
        // localPoint.x = 0; // 해당 버튼 위치에 따라서 조정할 필요가 있다
        // localPoint.y = 0;

        //StartCoroutine(GoUpAndDestroy(getGoldText));
        StartCoroutine(PlayDieAnimation(getGoldText));
    }

    // public IEnumerator GoUpAndDestroy(GameObject getGoldText)
    // {
    //     float time = 0;
    //     while (time < 0.3f)
    //     {
    //         time += Time.deltaTime;
    //         getGoldText.transform.position += new Vector3(0, 0.008f, 0);
    //         yield return null;
    //     }
    //     Debug.Log("destroy");
    //     Destroy(getGoldText);
    // }

    public void SlowDown(float slowPower)
    {
        if(!isSlowed)
        {
            this.speed -= slowPower;
            if(speed < 0)
            {
                speed = 0.1f;
            }
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

    public void OnSlowField(float slowPower)
    {
        if(!isOnSlowField)
        {
            this.speed -= slowPower;
            if(speed < 0)
            {
                speed = 0.1f;
            }
            isOnSlowField = true;
        }
    }

    public void OutSlowField()
    {
        this.speed = this.originalSpeed;
        isOnSlowField = false;
    }

    public IEnumerator PlayDieAnimation(GameObject getGoldText)
    {
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime * 2;
            head.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, head.GetComponent<SpriteRenderer>().color.a - 0.01f);
            body.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, body.GetComponent<SpriteRenderer>().color.a - 0.01f);

            getGoldText.transform.position += new Vector3(0, 0.008f, 0);

            yield return null; // while문 안에 yield return null을 넣어주는 이유는 while문이 돌면서 1프레임씩 기다려주기 위함
        }
        Destroy(gameObject);
        Destroy(getGoldText);
    }
}

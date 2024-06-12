using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System;

// HW1
// Enemy 종류 3가지 만들기 (Enemy1, Enemy2, Enemy3)
// BaseEnemy에 pause 기능 (speed 0으로 만들기)
// 천천히 없애기 (StartCoroutine 사용) StartCoroutine은 뭔가 새로운 가지를 만들어서 따로 실행하는 친구
// BaseEnemy 쪽에 Destroy 기능 넣기 (Animation 투명도 0으로 만들기 - sprite alpha값을 0으로 만들기

public abstract class BaseEnemy : MonoBehaviour
{

    public WayPoint[] path;
    public float moveDistance;
    private Vector2 prevPos;

    public GameObject head;
    public GameObject body;

    public Canvas canvasUI;
    public Camera mainCamera;

    private bool isMoving = false;
    public bool isDie = false;

    public bool isSlowed = false;
    public bool isOnSlowField = false;

    public float hp;
    public int gold;

    public float originalSpeed = 1.0f;
    public float speed;

    public int currentWayPointIndex = 0;
    public Vector2 currentWayPointPos;
    public LargeTurretSpace largeTurretSpace;

    GameObject map;

    Vector2 currentPos;
    Tuple<int, int> currentIndex;

    public List<Tuple<int, int>> wayPoints;

    public int rowIndex;
    public int columnIndex;

    private bool isPassedGoal = false;
    private float CountOfDie = 0f;

    //public float moveSpeed;


    public void Initialize(WayPoint[] path, Camera mainCamera, Canvas canvasUI, int currentWayPointIndex, Vector2 spawnPosition)
    {
        if (mainCamera.gameObject.GetComponent<Spawn1>())
        {
            this.path = path;
            this.currentWayPointIndex = currentWayPointIndex;
            this.transform.position = spawnPosition;
        }

        originalSpeed = speed;
        StaticValues.GetInstance().livingEnemyCount += 1;

        this.mainCamera = mainCamera;
        this.canvasUI = canvasUI;
    }

    // public void Initialize(Camera mainCamera, Canvas canvasUI, int currentWayPointIndex, Vector2 spawnPosition)
    // {
    //     originalSpeed = speed;
    //     StaticValues.GetInstance().livingEnemyCount += 1;

    //     this.mainCamera = mainCamera;
    //     this.canvasUI = canvasUI;

    //     this.currentWayPointIndex = currentWayPointIndex;
    //     this.transform.position = spawnPosition;
    //     FindWayPoint();
    //     SetMap();
    // }

    public void SetMap()
    {
        GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
        map = mapObject;

        var mainCamera = Camera.main;
        largeTurretSpace = mainCamera.GetComponent<LargeTurretSpace>();
    }

    public void MoveToNextWayPoint()
    {
        var mainCamera = Camera.main;
        if (mainCamera.gameObject.GetComponent<Spawn1>())
        {
            MoveWithNumberWayPoint();
        }
        else
        {
            MoveWithTupleWayPoint();
        }
    }

    public void MoveWithTupleWayPoint()
    {
        if (StaticValues.GetInstance().isPaused)
        {
            return;
        }

        //Debug.Log("맵 : " + map.name);

        currentPos = new Vector2(Mathf.Abs(largeTurretSpace.topLeftOfSpawnBox.x - body.transform.position.x), Mathf.Abs(largeTurretSpace.topLeftOfSpawnBox.y - body.transform.position.y));

        rowIndex = (int)(currentPos.y / largeTurretSpace.turretSpaceSize.y);
        columnIndex = (int)(currentPos.x / largeTurretSpace.turretSpaceSize.x);

        currentIndex = new Tuple<int, int>(rowIndex, columnIndex);

        if (!isPassedGoal)
        {

            if (wayPoints != null)
            {
                var wayPointColumnIndex = wayPoints[currentWayPointIndex].Item2;
                var wayPointRowIndex = wayPoints[currentWayPointIndex].Item1;
                var largeTurretSpaceLeftTopX = largeTurretSpace.topLeftOfSpawnBox.x;
                var largeTurretSpaceLeftTopY = largeTurretSpace.topLeftOfSpawnBox.y;
                var turretSpaceSize = largeTurretSpace.turretSpaceSize.x;

                //Debug.Log("목표 지점 : " + wayPointRowIndex + "," + wayPointColumnIndex);

                var wayPointX = wayPointColumnIndex * turretSpaceSize + largeTurretSpaceLeftTopX + turretSpaceSize / 2;
                var wayPointY = largeTurretSpaceLeftTopY - wayPointRowIndex * turretSpaceSize - turretSpaceSize / 2;

                // var topLeftDirection = new Vector2(largeTurretSpaceLeftTopX, largeTurretSpaceLeftTopY) - new Vector2(transform.position.x, transform.position.y);
                // Debug.DrawRay(transform.position, topLeftDirection, Color.green);

                var wayPointDirection = new Vector2(wayPointX, wayPointY) - new Vector2(transform.position.x, transform.position.y);
                Debug.DrawRay(transform.position, wayPointDirection, Color.blue);

                var destination = new Vector3(wayPointX, wayPointY, 0);
                this.transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);

                Vector3 directionToTarget = destination - transform.position;

                // Calculate the angle in degrees
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

                // Create a quaternion rotation around the Z-axis
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

                // Interpolate between current rotation and target rotation using Lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);


                if (new Vector2(transform.position.x, transform.position.y) == new Vector2(wayPointX, wayPointY))
                {
                    currentWayPointIndex += 1;
                    if (wayPoints.Count <= currentWayPointIndex)
                    {
                        isPassedGoal = true;
                    }
                }
            }
            else
            {
                transform.Translate(Time.deltaTime * speed, 0, 0);
            }
        }
        else // 만약 도착지점에 도착했다면
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
            CountOfDie += Time.deltaTime;
            //Debug.Log(CountOfDie);
            if (CountOfDie > 2f)
            {
                if (!isDie)
                {
                    var camera = GameObject.FindGameObjectWithTag("MainCamera");
                    var player = camera.GetComponent<Player>();
                    player.GotHit();
                    Die();
                }
            }
        }

        var ray = new Ray(this.transform.position, new Vector3(1, 0));
        var hits = Physics2D.RaycastAll(ray.origin, ray.direction, map.gameObject.GetComponent<SpriteRenderer>().bounds.size.x);

        var isTurretBlocking = false;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            var turretSpace = hit.transform.gameObject.GetComponent<TurretSpace>();

            if (turretSpace != null && !turretSpace.GetComponent<BoxCollider2D>().isTrigger)
            {
                isTurretBlocking = true;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * map.gameObject.GetComponent<SpriteRenderer>().bounds.size.x, Color.red);

        if (isTurretBlocking)
        {

        }
    }

    public void MoveWithNumberWayPoint()
    {
        if (this.currentWayPointIndex < this.path.Length)
        {
            if (prevPos == null)
            {
                prevPos = this.transform.position;
            }
            moveDistance += Vector2.Distance(prevPos, this.transform.position);
            prevPos = this.transform.position;

            //Debug.Log("currentIndex : " + currentWayPointIndex);
            Debug.Log("current ", this.path[this.currentWayPointIndex]);
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
                if (this.currentWayPointIndex == this.path.Length)
                {
                    var camera = GameObject.FindGameObjectWithTag("MainCamera");
                    var player = camera.GetComponent<Player>();

                    player.GotHit();
                    StaticValues.GetInstance().livingEnemyCount -= 1;
                    Destroy(this.gameObject);
                    if (StaticValues.GetInstance().hp > 0)
                    {
                        var isEnemyCleared = StaticValues.GetInstance().livingEnemyCount == 0;
                        if (isEnemyCleared && Camera.main.GetComponent<Player>().isLastLevel)
                        {
                            // Game Over?
                            Camera.main.GetComponent<Player>().StageClear();
                            Debug.Log(isEnemyCleared);
                        }
                    }
                }
            }
        }
    }

    public void FindWayPoint()
    {
        // BFS로 다음 waypoint를 정해야 한다, 

        //bool isReached = true;

        var spaceDict = AdditionalStaticValuesForGame2.GetInstance().turretSpace;

        var hasCheckedAvailability = new Dictionary<Tuple<int, int>, Boolean>();

        Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
        var visitingBlock = currentIndex;
        hasCheckedAvailability[visitingBlock] = true;
        queue.Enqueue(visitingBlock);

        int maxTurretSpaceColumnCount = AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceColumnCount;
        int maxTurretSpaceRowCount = AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceRowCount;

        var loopCount = 0;
        const int maxLoopCount = 200;

        var previousIndexForDestination = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

        while (visitingBlock.Item2 < maxTurretSpaceColumnCount - 1) // 목적지(맨 오른쪽 열)에 도착할때까지 계속 반복
        {
            loopCount += 1;
            if (loopCount > maxLoopCount)
            {
                break;
            }
            visitingBlock = queue.Dequeue();

            var leftBlock = new Tuple<int, int>(visitingBlock.Item1, visitingBlock.Item2 - 1);
            var rightBlock = new Tuple<int, int>(visitingBlock.Item1, visitingBlock.Item2 + 1);
            var upBlock = new Tuple<int, int>(visitingBlock.Item1 - 1, visitingBlock.Item2);
            var downBlock = new Tuple<int, int>(visitingBlock.Item1 + 1, visitingBlock.Item2);

            bool hasCheckedLeftBlock = hasCheckedAvailability.ContainsKey(leftBlock) == true;
            bool hasCheckedRightBlock = hasCheckedAvailability.ContainsKey(rightBlock) == true;
            bool hasCheckedUpBlock = hasCheckedAvailability.ContainsKey(upBlock) == true;
            bool hasCheckedDownBlock = hasCheckedAvailability.ContainsKey(downBlock) == true;

            bool isLeftBlockOutside = spaceDict.TryGetValue(leftBlock, out var isLeftWithTurret) == false;
            bool isRightBlockOutside = spaceDict.TryGetValue(rightBlock, out var isRightWithTurret) == false;
            bool isUpBlockOutside = spaceDict.TryGetValue(upBlock, out var isUpWithTurret) == false;
            bool isDownBlockOutside = spaceDict.TryGetValue(downBlock, out var isDownWithTurret) == false;

            bool isLeftBlockNotValid = isLeftBlockOutside || isLeftWithTurret || hasCheckedLeftBlock;
            bool isRightBlockNotValid = isRightBlockOutside || isRightWithTurret || hasCheckedRightBlock;
            bool isUpBlockNotValid = isUpBlockOutside || isUpWithTurret || hasCheckedUpBlock;
            bool isDownBlockNotValid = isDownBlockOutside || isDownWithTurret || hasCheckedDownBlock;

            if (!isLeftBlockNotValid)
            {
                queue.Enqueue(leftBlock);
                hasCheckedAvailability[leftBlock] = true;
                previousIndexForDestination[leftBlock] = visitingBlock;
            }
            if (!isRightBlockNotValid)
            {
                queue.Enqueue(rightBlock);
                hasCheckedAvailability[rightBlock] = true;
                previousIndexForDestination[rightBlock] = visitingBlock;
            }
            if (!isUpBlockNotValid)
            {
                queue.Enqueue(upBlock);
                hasCheckedAvailability[upBlock] = true;
                previousIndexForDestination[upBlock] = visitingBlock;
            }
            if (!isDownBlockNotValid)
            {
                queue.Enqueue(downBlock);
                hasCheckedAvailability[downBlock] = true;
                previousIndexForDestination[downBlock] = visitingBlock;
            }
        }

        List<Tuple<int, int>> pathToDestination = new List<Tuple<int, int>>();
        loopCount = 0;

        while (previousIndexForDestination[visitingBlock] != currentIndex)
        {
            //Debug.Log(visitingBlock);
            loopCount += 1;
            if (loopCount > maxLoopCount)
            {
                break;
            }
            pathToDestination.Add(previousIndexForDestination[visitingBlock]);
            visitingBlock = previousIndexForDestination[visitingBlock];
        }

        pathToDestination.Reverse();
        wayPoints = pathToDestination;
        currentWayPointIndex = 0;

        // foreach (var space in largeTurretSpace.turretSpaces.Values)
        // {
        //     space.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        // }

        // foreach (var path in pathToDestination)
        // {
        //     largeTurretSpace.turretSpaces[path].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        // }
    }

    public void Pause()
    {
        this.speed = 0f;
    }

    public void GotHit(float attackDamage)
    {
        this.hp -= attackDamage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //Destroy(this.gameObject);
        if (!isDie)
        {
            isDie = true;
            StaticValues.GetInstance().gold += this.gold;
            var isLastEnemy = StaticValues.GetInstance().livingEnemyCount == 1;
            if (isLastEnemy && Camera.main.GetComponent<Player>().isLastLevel)
            {
                // Game Over?
                Camera.main.GetComponent<Player>().StageClear();
                Debug.Log(isLastEnemy);
            }
            StaticValues.GetInstance().livingEnemyCount -= 1;
            ShowGetGold(this.gold);
            Debug.Log(StaticValues.GetInstance().livingEnemyCount);
        }
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
        if (!isSlowed)
        {
            this.speed -= slowPower;
            if (speed < 0)
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
        if (!isOnSlowField)
        {
            this.speed -= slowPower;
            if (speed < 0)
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
        //Debug.Log();
        Destroy(getGoldText);
        Destroy(gameObject);
    }
}

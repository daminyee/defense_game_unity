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

// 6월 22일(토요일) 밤
// 1. BFS때문에 생기는 마지막 통과지점 버그 해결하기 -> BFS를 조금 수정할 필요가 있을 것
// 2. 마지막 GAME3 생각해보기 -> 어떻게 할지

// 7월 17일 수업
// 1. 적 스폰되는 거를 좀 줄이고 여러번 나오게 
// 2. 적이 이동하는 거리를 빨갛게 표현해보기 
// (3은 다음에)
// 3. 랜덤으로 생성되는 path를 저장하고 다시 불러오게끔 해보기 
//      -> 만약 주입한 argument가 없은 진짜 랜덤으로
// 시작점 -> 끝점 모든 row index column index array string(혹은 다른 방식으로)으로 저장하고 불러오는 기능 넣어보기
// 4. 위 방법들로 반복적으로 테스트해보고 한번 해결해보기

// 7월 21일 수요일
// 숙제1: 새로 태어난 적들이 이상하게 움직이는 것 해결
// 숙제2: 게임3 잘 작동하는지 확인하고 문제가 있으면 해결하기

public abstract class BaseEnemy : MonoBehaviour
{
    public GameObject head;
    public GameObject body;
    public Canvas canvasUI;
    public Camera mainCamera;
    public bool isDie = false;
    public bool isSlowed = false;
    public bool isOnSlowField = false;

    public float hp;
    public int gold;
    public float originalSpeed = 1.0f;
    public float speed;

    //**Game 1**
    public WayPoint[] path;
    public float targetPriority;
    private Vector2 prevPos;

    //**Game2 && Game3**
    GameObject map;
    public LargeTurretSpace largeTurretSpace;

    public List<Tuple<int, int>> wayPoints;
    public int currentWayPointIndex = 0;
    public Vector2 currentWayPointPos;
    Tuple<int, int> currentIndex;
    Vector2 currentPos;

    public int rowIndex;
    public int columnIndex;

    public int startingPointY;

    private bool hasPassedLastWayPoint = false;
    public bool hasPassed = false;

    // 정의된 5개는 parameter 파라미터
    public void Initialize(WayPoint[] path, Camera mainCamera, Canvas canvasUI, int currentWayPointIndex, Vector2 spawnPosition)
    {
        originalSpeed = speed;
        StaticValues.GetInstance().livingEnemyCount += 1;

        this.mainCamera = mainCamera;
        this.canvasUI = canvasUI;

        if (mainCamera.gameObject.GetComponent<Spawn1>())
        {
            this.path = path;
            this.currentWayPointIndex = currentWayPointIndex;
            this.transform.position = spawnPosition;
        }
        else if (mainCamera.gameObject.GetComponent<Spawn2>())
        {
            SetMap();
            // 여기부분이 문제다
            currentIndex = new Tuple<int, int>(startingPointY, 0);
            MoveWithTupleWayPoint();
            FindWayPoint();
        }
    }

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
            MoveWithNumberWayPoint(); //Game1
        }
        else
        {
            MoveWithTupleWayPoint(); //Game2 & Game3
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
            targetPriority += Vector2.Distance(prevPos, this.transform.position);
            prevPos = this.transform.position;

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
                        }
                    }
                }
            }
        }
    }

    public void MoveWithTupleWayPoint()
    {
        if (StaticValues.GetInstance().isPaused)
        {
            return;
        }

        var isEnemyInMap = largeTurretSpace.topLeftOfSpawnBox.x < this.transform.position.x
            && this.transform.position.x < largeTurretSpace.topLeftOfSpawnBox.x + largeTurretSpace.spawnBoxSize.x
            && largeTurretSpace.topLeftOfSpawnBox.y > this.transform.position.y
            && this.transform.position.y > largeTurretSpace.topLeftOfSpawnBox.y - largeTurretSpace.spawnBoxSize.y;

        if (!isEnemyInMap)
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
            return;
        }

        currentPos = new Vector2(Mathf.Abs(largeTurretSpace.topLeftOfSpawnBox.x - body.transform.position.x), Mathf.Abs(largeTurretSpace.topLeftOfSpawnBox.y - body.transform.position.y));

        rowIndex = (int)(currentPos.y / largeTurretSpace.turretSpaceSize.y);
        columnIndex = (int)(currentPos.x / largeTurretSpace.turretSpaceSize.x);

        currentIndex = new Tuple<int, int>(rowIndex, columnIndex);

        if (!hasPassedLastWayPoint)
        {
            if (wayPoints != null)
            {
                var wayPointColumnIndex = wayPoints[currentWayPointIndex].Item2;
                var wayPointRowIndex = wayPoints[currentWayPointIndex].Item1;

                var largeTurretSpaceLeftTopX = largeTurretSpace.topLeftOfSpawnBox.x;
                var largeTurretSpaceLeftTopY = largeTurretSpace.topLeftOfSpawnBox.y;
                var turretSpaceSize = largeTurretSpace.turretSpaceSize.x;

                var wayPointX = wayPointColumnIndex * turretSpaceSize + largeTurretSpaceLeftTopX + turretSpaceSize / 2;
                var wayPointY = largeTurretSpaceLeftTopY - wayPointRowIndex * turretSpaceSize - turretSpaceSize / 2;

                var destination = new Vector3(wayPointX, wayPointY, 0);
                this.transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);

                Vector3 directionToTarget = destination - transform.position;

                // Calculate the angle in degrees
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

                // Create a quaternion rotation around the Z-axis
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

                // Interpolate between current rotation and target rotation using Lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);


                if (new Vector2(transform.position.x, transform.position.y) == new Vector2(wayPointX, wayPointY)) //현재 목적지 도착
                {
                    currentWayPointIndex += 1;
                    if (currentWayPointIndex == wayPoints.Count) //최종 목적지 도달
                    {
                        hasPassedLastWayPoint = true;
                    }
                }
            }
            else
            {
                transform.Translate(Time.deltaTime * speed, 0, 0);
                targetPriority += Vector2.Distance(prevPos, this.transform.position);
            }
        }
        else
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }
        if (largeTurretSpace.topLeftOfSpawnBox.x + largeTurretSpace.spawnBoxSize.x < this.transform.position.x) // 만약 도착지점에 도착했다면
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
            hasPassed = true;
            if (!isDie)
            {
                StartCoroutine(CountForDie());
            }
        }
    }

    IEnumerator CountForDie()
    {
        yield return new WaitForSeconds(2);

        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        var player = camera.GetComponent<Player>();
        player.GotHit();
        Die();
    }

    public void FindWayPoint()
    {
        // BFS로 다음 waypoint를 정해야 한다, 

        var spaceDict = AdditionalStaticValuesForGame2.GetInstance().turrets;

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


        while (visitingBlock.Item2 <= maxTurretSpaceColumnCount - 1) // 목적지(맨 오른쪽 열)에 도착할때까지 계속 반복
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
                if (!largeTurretSpace.turretSpaces[leftBlock].isNotPath)
                {
                    queue.Enqueue(leftBlock);
                    hasCheckedAvailability[leftBlock] = true;
                    previousIndexForDestination[leftBlock] = visitingBlock;
                }
            }
            if (!isRightBlockNotValid)
            {
                if (!largeTurretSpace.turretSpaces[rightBlock].isNotPath)
                {
                    queue.Enqueue(rightBlock);
                    hasCheckedAvailability[rightBlock] = true;
                    previousIndexForDestination[rightBlock] = visitingBlock;
                }
            }
            if (!isUpBlockNotValid)
            {
                if (!largeTurretSpace.turretSpaces[upBlock].isNotPath)
                {
                    queue.Enqueue(upBlock);
                    hasCheckedAvailability[upBlock] = true;
                    previousIndexForDestination[upBlock] = visitingBlock;
                }
            }
            if (!isDownBlockNotValid)
            {
                if (!largeTurretSpace.turretSpaces[downBlock].isNotPath)
                {
                    queue.Enqueue(downBlock);
                    hasCheckedAvailability[downBlock] = true;
                    previousIndexForDestination[downBlock] = visitingBlock;
                }
            }
            if (visitingBlock.Item2 == maxTurretSpaceColumnCount - 1) // 목적지에 도달했을 때
            {
                // // 경로를 계산할때 여기의 rightBlock은 참조하지 않음
                previousIndexForDestination[rightBlock] = visitingBlock;
                visitingBlock = rightBlock;
                break;
            }
        }

        List<Tuple<int, int>> pathToDestination = new List<Tuple<int, int>>();
        loopCount = 0;

        while (previousIndexForDestination[visitingBlock] != currentIndex)
        {
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

        targetPriority = -wayPoints.Count;
        currentWayPointIndex = 0;
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
        StartCoroutine(PlayDieAnimation(getGoldText));
    }

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

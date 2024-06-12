using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 5월 12일
// 1. 적이 바라보는 방향과 무관하게 적의 오른편이 Debug Ray 하세요
// 2. Queue라는 자료구조를 공부하고 오기 (그리고 설명하기), 유니티 Start Script하나 써서 (직접 만들기) 코드로 작동하는 거 하나 (아무거나 해도 되는데 문제 설명) 예제 만들어오기

// BFS (Breadth First Search)
// DFS (Depth First Search)

// 5월 28일 숙제
// 1. 적의 현재 spawn box 정수 위치를 알아내기 
// 2. BFS로 적이 끝에 도달하려면 갈 수 있는 경로를 찾아내기 (처음 끝에 도달한 경로를 Debug하기)

// 6월 4일 숙제
// 1. 적 움직이는거 전체 구현 다하기 (완벽하게) -> TestEnemy.cs

// 6월 9일 숙제
// 1. 진짜 적을 저 waypoint를 사용하게끔 도입하기 (복습한다는 생각) ***중요***
// 2. waypoint가 마지막 구간에 도달할 때 오른쪽으로 빠져나가게끔 
// 3. 적이 마지막 구간을 통과하면 체력이 1 깎이는 거 구현 + 적이 사라지는 거 구현

public class TestEnemy : MonoBehaviour
{
    LargeTurretSpace largeTurretSpace;

    GameObject map;

    Vector2 currentPos;
    Tuple<int, int> currentIndex;

    public List<Tuple<int, int>> wayPoints;
    public Vector2 currentWayPointPos;
    public int currentWayPointIndex = 0;

    int rowIndex;
    int columnIndex;

    public float moveSpeed;

    void Start()
    {
        GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
        map = mapObject;

        var mainCamera = Camera.main;
        largeTurretSpace = mainCamera.GetComponent<LargeTurretSpace>();
        // 여기에 Queue 예제 코드 작성
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector2(Mathf.Abs(largeTurretSpace.topLeftOfSpawnBox.x - this.transform.position.x), Mathf.Abs(largeTurretSpace.topLeftOfSpawnBox.y - this.transform.position.y));

        rowIndex = (int)(currentPos.y / largeTurretSpace.turretSpaceSize.y);
        columnIndex = (int)(currentPos.x / largeTurretSpace.turretSpaceSize.x);

        currentIndex = new Tuple<int, int>(rowIndex, columnIndex);

        //Debug.Log(wayPoints[currentWayPointIndex]);



        if (wayPoints != null)
        {
            var wayPointColumnIndex = wayPoints[currentWayPointIndex].Item2;
            var wayPointRowIndex = wayPoints[currentWayPointIndex].Item1;
            var largeTurretSpaceLeftTopX = largeTurretSpace.topLeftOfSpawnBox.x;
            var largeTurretSpaceLeftTopY = largeTurretSpace.topLeftOfSpawnBox.y;
            var turretSpaceSize = largeTurretSpace.turretSpaceSize.x;

            var wayPointX = wayPointColumnIndex * turretSpaceSize + largeTurretSpaceLeftTopX + turretSpaceSize / 2;
            var wayPointY = largeTurretSpaceLeftTopY - wayPointRowIndex * turretSpaceSize - turretSpaceSize / 2;

            var topLeftDirection = new Vector2(largeTurretSpaceLeftTopX, largeTurretSpaceLeftTopY) - new Vector2(transform.position.x, transform.position.y);


            Debug.DrawRay(transform.position, topLeftDirection, Color.green);

            var wayPointDirection = new Vector2(wayPointX, wayPointY) - new Vector2(transform.position.x, transform.position.y);
            Debug.DrawRay(transform.position, wayPointDirection, Color.blue);

            var destination = new Vector3(wayPointX, wayPointY, 0);
            this.transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);

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
            }
        }
        else
        {
            transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
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

        foreach (var space in largeTurretSpace.turretSpaces.Values)
        {
            space.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        foreach (var path in pathToDestination)
        {
            largeTurretSpace.turretSpaces[path].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        }
    }
}

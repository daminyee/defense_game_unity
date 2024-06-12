using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 5월 14일까지
// 1. 가장 기초적인 한 열(column)이 다 막혔을 때 설치가 안되게 하기
// 2. Breadth First Search 복습 및 설명하기
// 다다음 숙제는 직접 혼자서 BFS하는 겁니다. -> 본인이 하고 싶으면 2번에 더해 BFS도 구현하는 것도 나쁘지 않음

// 5월 19일 
// 1. BFS 복습하기 (다음주에 BFS 구현하기)
// 2. BFS 완성하기 (일요일까지 무조건 완성하지!) -> 본인의 힘을 꼭 해보기 (딴 거 참고해도 된다 - 인터넷 검색 상관없음)
// -> 설치가능여부를 판단해서 설치될 터렛이 적이 끝점에 도달하는 것을 막는다면 설치 못하게 하기!
// 3. 첫번째 열은 설치못하게 하기 

// 5월 26일 숙제
// 1. BFS 코드 완벽이해하기 -> 본인을 위해서 꼭 이해하고, 테스트하겠음 (각각 코드가 뭐하는지 물어보겠음)
// 2. 설치 전에 가능한지 안 가능한지 여부로 Install Turret 방지하기
public class TurretOption : ClickDetector
{
    public GameObject draggingTurretObject = null;
    public GameObject turretPrefab;
    public Canvas canvasUI;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public override void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0;
        if (draggingTurretObject == null)
        {
            draggingTurretObject = Instantiate(turretPrefab, worldPoint, Quaternion.identity);
            draggingTurretObject.GetComponent<BaseTurret>().Initialize(true, this.mainCamera, this.canvasUI);
        }
        else
        {
            draggingTurretObject.transform.position = worldPoint;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0;
        draggingTurretObject.transform.position = worldPoint;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        var draggingTurret = draggingTurretObject.GetComponent<BaseTurret>();

        if (this.draggingTurretObject != null)
        {
            if (!draggingTurret.isInValidSpace || (draggingTurret.turretSpace != null && draggingTurret.turretSpace.isInstalled))
            {
                Destroy(draggingTurretObject);
            }
            else
            {
                if (StaticValues.GetInstance().gold < draggingTurret.price)
                {
                    Destroy(draggingTurretObject);
                }
                else
                {
                    var isBlockingEnemyPath = false;
                    Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(rayPos, Vector2.zero);

                    foreach (var hit in hits)
                    {
                        BaseEnemy enemy = hit.collider.GetComponent<BaseEnemy>();
                        if (enemy != null)
                        {
                            isBlockingEnemyPath = true;
                        }
                    }


                    var currentPos = new Tuple<int, int>(draggingTurret.turretSpace.rowIndex, draggingTurret.turretSpace.columnIndex);

                    var spaceDict = AdditionalStaticValuesForGame2.GetInstance().turretSpace;
                    spaceDict[currentPos] = true;

                    var hasCheckedAvailability = new Dictionary<Tuple<int, int>, Boolean>();
                    // BFS로 적이 끝점(오른쪽에) 도달 가능한지 판단 알고리즘 구현하세요
                    Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
                    var visitingBlock = new Tuple<int, int>(0, 0);
                    hasCheckedAvailability[visitingBlock] = true;
                    queue.Enqueue(visitingBlock);

                    int maxTurretSpaceColumnCount = AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceColumnCount;
                    int maxTurretSpaceRowCount = AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceRowCount;

                    // Debug.Log("column" + maxTurretSpaceColumnCount);
                    // Debug.Log("row" + maxTurretSpaceRowCount);

                    // 이걸 어떻게 반복해서 돌지
                    // Hint: While문을 써야 한다. While문의 종료 조건을 걸어야 한다. 1) 목적지에 도달했을 때 2) Queue가 비었을 때 (queue.Count == 0)

                    if (draggingTurret.turretSpace.columnIndex == 0) // 첫째줄 설치방지
                    {
                        isBlockingEnemyPath = true;
                    }

                    var loopCount = 0;
                    const int maxLoopCount = 200;



                    while (visitingBlock.Item2 < maxTurretSpaceColumnCount - 1) // 목적지(맨 오른쪽 열)에 도착할때까지 계속 반복
                    {
                        loopCount += 1;
                        if (loopCount > maxLoopCount)
                        {
                            Debug.Log("did break?");
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

                        // TryGetValue 아래 코드와 동일
                        // bool isLeftBlockOutside = spaceDict.ContainsKey(leftBlock) == false;
                        // bool isLeftWithTurret = false;
                        // if (!isLeftBlockOutside)
                        // {
                        //     isLeftWithTurret = spaceDict[leftBlock];
                        // }

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
                        }
                        if (!isRightBlockNotValid)
                        {
                            queue.Enqueue(rightBlock);
                            hasCheckedAvailability[rightBlock] = true;
                        }
                        if (!isUpBlockNotValid)
                        {
                            queue.Enqueue(upBlock);
                            hasCheckedAvailability[upBlock] = true;
                        }
                        if (!isDownBlockNotValid)
                        {
                            queue.Enqueue(downBlock);
                            hasCheckedAvailability[downBlock] = true;
                        }
                        if (queue.Count == 0) // 만약 갈 수 있는 모든 곳을 방문하고도 목적지에 도달하지 못했다면
                        {
                            isBlockingEnemyPath = true;
                            break;
                        }
                    }

                    spaceDict[currentPos] = false;

                    if (!isBlockingEnemyPath)
                    {
                        draggingTurret.InstallTurret();
                        //Debug.Log("설치됨");
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach (GameObject enemy in enemies)
                        {
                            //enemy.GetComponent<TestEnemy>().FindWayPoint();
                            enemy.GetComponent<BaseEnemy>().FindWayPoint();
                        }
                    }

                    else
                    {
                        Destroy(draggingTurretObject);
                        //Debug.Log("설치안됨");
                    }
                    // int maxTurretSpaceCount = 0;
                    // int installedTurretSpaceCount = 0;
                    // var currentTurretKey = new Tuple<int, int>(draggingTurret.turretSpace.rowIndex, draggingTurret.turretSpace.columnIndex);
                    // foreach (var key in AdditionalStaticValuesForGame2.GetInstance().turretSpace.Keys)
                    // {
                    //     if (key.Item2 == currentTurretKey.Item2)
                    //     {
                    //         maxTurretSpaceCount += 1;
                    //         //Debug.Log(maxTurretSpaceCount);
                    //         if (AdditionalStaticValuesForGame2.GetInstance().turretSpace[key] == true)
                    //         {
                    //             installedTurretSpaceCount += 1;
                    //             //Debug.Log(installedTurretSpaceCount);
                    //         }
                    //     }
                    // }
                    // if (installedTurretSpaceCount < maxTurretSpaceCount - 1)
                    // {
                    //     draggingTurret.InstallTurret();
                    //     maxTurretSpaceCount = 0;
                    //     installedTurretSpaceCount = 0;
                    //     Debug.Log("설치됨");
                    // }
                }
            }
        }
        draggingTurretObject = null;
    }

    private Vector3 ScreenToWorldPoint(Vector2 screenPoint)
    {
        return mainCamera.ScreenToWorldPoint(screenPoint);
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 6/30 오전 10시 30분 수업
// 숙제 DFS로 길 2개 그리는 것 완성하기
// 가능하면 적이 그 길을 따라가게 하는 거를 해보기 (스폰 위치를 DFS 시작점으로 할 필요가 있음 -> 시작점, 끝점을 따로 기록을 해야 것)
public class RandomPath : MonoBehaviour
{
    public GameObject spawnBox;
    public Dictionary<int, int> randomNums = new Dictionary<int, int>();
    int count = 0;
    void Start()
    {

    }

    void Update()
    {

    }

    public void MakeRandomPath(string path = null)
    {
        Tuple<int, int> visitingBlock = new Tuple<int, int>(0, 0);
        var spaceDict = AdditionalStaticValuesForGame2.GetInstance().turrets;
        int maxTurretSpaceColumnCount = AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceColumnCount;
        int maxTurretSpaceRowCount = AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceRowCount;

        int startingPoint = UnityEngine.Random.Range(0, maxTurretSpaceRowCount);
        randomNums[count] = startingPoint;

        count += 1;

        visitingBlock = new Tuple<int, int>(startingPoint, visitingBlock.Item2);

        var loopCount = 0;
        const int maxLoopCount = 200;

        while (visitingBlock.Item2 < maxTurretSpaceColumnCount + 1)
        {
            loopCount += 1;
            if (loopCount > maxLoopCount)
            {
                break;
            }

            GameObject[] turretSpaces = GameObject.FindGameObjectsWithTag("TurretSpace");
            foreach (var spaceObject in turretSpaces)
            {
                var turretSpace = spaceObject.GetComponent<TurretSpace>();
                if (turretSpace != null)
                {
                    if (visitingBlock.Equals(new Tuple<int, int>(turretSpace.rowIndex, turretSpace.columnIndex)))
                    {
                        turretSpace.TurnOnIsTrigger();
                        turretSpace.isInstalled = true;
                        turretSpace.isNotPath = false;
                        spaceObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1f);
                    }
                }
            }

            int randomNum = UnityEngine.Random.Range(0, 3);

            Tuple<int, int> rightBlock = new Tuple<int, int>(visitingBlock.Item1, visitingBlock.Item2 + 1);
            Tuple<int, int> upBlock = new Tuple<int, int>(visitingBlock.Item1 - 1, visitingBlock.Item2);
            Tuple<int, int> downBlock = new Tuple<int, int>(visitingBlock.Item1 + 1, visitingBlock.Item2);

            bool isRightBlockNotValid = spaceDict.ContainsKey(rightBlock) == false;
            bool isUpBlockNotValid = spaceDict.ContainsKey(upBlock) == false;
            bool isDownBlockNotValid = spaceDict.ContainsKey(downBlock) == false;

            switch (randomNum)
            {
                case 0:
                    if (!isRightBlockNotValid)
                    {
                        visitingBlock = rightBlock;
                    }
                    break;
                case 1:
                    if (!isUpBlockNotValid)
                    {
                        visitingBlock = upBlock;
                    }
                    break;
                case 2:
                    if (!isDownBlockNotValid)
                    {
                        visitingBlock = downBlock;
                    }
                    break;
            }
        }
    }
}

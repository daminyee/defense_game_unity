using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 숙제 5월 5일까지 
// 1. 복습 (spawn하는 로직을 for loop 두 개 왜 썼는지 이해하기) - 이중 for loop (작동 원리)
// 2. 에러 해결하세요 (waypoint) -> 우리는 여기서 waypoint안씀, 적 스폰하는 로직이 달라질 것이다 -> 적 스폰은 당분간 안할거임
// 3. 모든 터렛들 가운데에 정렬되게끔 (x축 y축 모두) -> turret space의 여백이 적절하게 나뉘어지게끔 코드 구현 (수학적인 계산이 필요)


// 숙제 5월 8일 (시간 9시 30분)
// 1. AdditionalStaticValuesForGame2 에서 만든 Add.., Subtract.. 를 활용해서 설치가능 여부를 판단하는 로직을 구현하세요
// 2. 적이 바라보는 방향을 Debug Ray로 표시하게 하기 (적이 바라보는 방향을 알 수 있어야 함)
// -> 적이 바라보는 방향에 collider (trigger가 안 켜진 collider) 가 있으면 Debug Log하게 하기

public class LargeTurretSpace : MonoBehaviour
{
    public GameObject spawnBox;
    public Vector2 spawnBoxSize = new Vector2();

    public GameObject turretSpace;
    public GameObject wall;
    public Vector2 turretSpaceSize = new Vector2();

    public Vector2 topLeftOfSpawnBox = new Vector2();

    public Dictionary<Tuple<int, int>, TurretSpace> turretSpaces = new Dictionary<Tuple<int, int>, TurretSpace>();

    int turretSpaceCountX;
    int turretSpaceCountY;

    void Start()
    {
        spawnBoxSize.x = spawnBox.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        spawnBoxSize.y = spawnBox.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;

        turretSpaceSize.x = this.turretSpace.GetComponent<SpriteRenderer>().bounds.size.x;
        turretSpaceSize.y = this.turretSpace.GetComponent<SpriteRenderer>().bounds.size.y;

        turretSpaceCountX = (int)(spawnBoxSize.x / turretSpaceSize.x);
        turretSpaceCountY = (int)(spawnBoxSize.y / turretSpaceSize.y);

        float restDistanceX = spawnBoxSize.x % turretSpaceSize.x;
        float restDistanceY = spawnBoxSize.y % turretSpaceSize.y;

        // Debug.Log("countX:" + turretSpaceCountX);
        // Debug.Log("countY:" + turretSpaceCountY);

        AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceColumnCount = turretSpaceCountX;
        AdditionalStaticValuesForGame2.GetInstance().maxTurretSpaceRowCount = turretSpaceCountY;

        // 시작점 찾기
        this.topLeftOfSpawnBox = new Vector2(spawnBox.gameObject.transform.position.x - spawnBoxSize.x / 2 + restDistanceX / 2, this.gameObject.transform.position.y + spawnBoxSize.y / 2 - restDistanceY / 2);

        // i? x
        for (int i = 0; i < turretSpaceCountX; i++)
        {
            // j? y
            for (int j = 0; j < turretSpaceCountY; j++)
            {
                var position = new Vector2(topLeftOfSpawnBox.x + i * turretSpaceSize.x,
                                           topLeftOfSpawnBox.y - j * turretSpaceSize.y);
                position.x += turretSpaceSize.x / 2;
                position.y -= turretSpaceSize.y / 2;
                var installedTurretSpace = Instantiate(turretSpace, position, Quaternion.identity);
                installedTurretSpace.GetComponent<TurretSpace>().columnIndex = i;
                installedTurretSpace.GetComponent<TurretSpace>().rowIndex = j;

                AdditionalStaticValuesForGame2.GetInstance().SetTurretInstallStatus(j, i, false);
                Tuple<int, int> index = new Tuple<int, int>(j, i);
                turretSpaces[index] = installedTurretSpace.GetComponent<TurretSpace>();
                installedTurretSpace.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
            }
        }

        GameObject topWall = Instantiate(wall, new Vector2(spawnBox.transform.position.x, spawnBoxSize.y / 2 - restDistanceY / 4), Quaternion.identity);
        GameObject bottomWall = Instantiate(wall, new Vector2(spawnBox.transform.position.x, -spawnBoxSize.y / 2 + restDistanceY / 4), Quaternion.identity);

        var newLocalX = spawnBoxSize.x / topWall.GetComponent<SpriteRenderer>().bounds.size.x;
        var newLocalY = restDistanceY / 2 / topWall.GetComponent<SpriteRenderer>().bounds.size.y;
        topWall.transform.localScale = new Vector3(newLocalX, newLocalY, 1);
        bottomWall.transform.localScale = new Vector3(newLocalX, newLocalY, 1);
    }

    void Update()
    {

    }
}

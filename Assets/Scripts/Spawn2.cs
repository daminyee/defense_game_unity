using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 숙제: 7월 7일
// 1. Spawn3 기존 스크립트 되살리기 (random하게 spawn하는 스크립트)
// 2. Spawn2 스크립트 완성 (random path의 starting point
// 3. spawn을 random path의 시작점에서 조금 왼쪽으로 치우친? 위치에서 시작하게 하기

public class Spawn2 : BaseSpawn
{
    public GameObject spawnBox;
    private float spawnBoxTopPos;
    private float spawnBoxBottomPos;
    public int startingPoint;

    public WayPoint[] path;

    public override void Spawn(EnemyType enemy)
    {
        spawnBoxTopPos = spawnBox.transform.position.y + spawnBox.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        spawnBoxBottomPos = spawnBox.transform.position.y - spawnBox.GetComponent<SpriteRenderer>().bounds.size.y / 2;


        float startingX = spawnBox.transform.position.x - 1f;
        float startingY = Random.Range(spawnBoxBottomPos, spawnBoxTopPos);
        float startingZ = spawnBox.transform.position.z;

        var randomPath = GetComponent<RandomPath>();
        if (randomPath != null)
        {
            int randomNum = UnityEngine.Random.Range(0, 2);
            startingPoint = randomPath.randomNums[randomNum];
            float turretSpaceSizeY = GetComponent<LargeTurretSpace>().turretSpaceSize.y;

            startingY = spawnBoxTopPos - startingPoint * turretSpaceSizeY - turretSpaceSizeY / 2;
            //Debug.Log($"currentStartingPoint : {startingPoint}");
        }
        var startPoint = new Vector3(startingX, startingY, startingZ);
        GameObject newEnemy = null;
        switch (enemy)
        {
            case EnemyType.Normal:
                newEnemy = Instantiate(normalEnemyPrefab, startPoint, Quaternion.identity);
                newEnemy.GetComponent<BaseEnemy>().startingPointY = startingPoint;
                break;
            case EnemyType.Fast:
                newEnemy = Instantiate(fastEnemyPrefab, startPoint, Quaternion.identity);
                newEnemy.GetComponent<BaseEnemy>().startingPointY = startingPoint;
                break;
            case EnemyType.Tanking:
                newEnemy = Instantiate(tankingEnemyPrefab, startPoint, Quaternion.identity);
                newEnemy.GetComponent<BaseEnemy>().startingPointY = startingPoint;
                break;
            case EnemyType.MiniBoss:
                newEnemy = Instantiate(MiniBossEnemyPrefab, startPoint, Quaternion.identity);
                newEnemy.GetComponent<BaseEnemy>().startingPointY = startingPoint;
                break;
            case EnemyType.Multiple:
                newEnemy = Instantiate(multipleEnemyPrefab, startPoint, Quaternion.identity);
                newEnemy.GetComponent<BaseEnemy>().startingPointY = startingPoint;
                break;
            case EnemyType.Shield:
                newEnemy = Instantiate(shieldEnemyPrefab, startPoint, Quaternion.identity);
                newEnemy.GetComponent<BaseEnemy>().startingPointY = startingPoint;
                break;
        }
        if (newEnemy == null)
        {
            return;
        }

        // argument 실제로 넣어주는 값 
        newEnemy.GetComponent<BaseEnemy>().Initialize(this.path, mainCamera, UI_Parent, 0, path[0].transform.position);
    }
}

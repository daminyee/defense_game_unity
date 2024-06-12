using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn2 : BaseSpawn
{
    public GameObject spawnBox;
    private float spawnBoxTopPos;
    private float spawnBoxBottomPos;

    public WayPoint[] path;

    public override void Spawn(EnemyType enemy)
    {
        spawnBoxTopPos = spawnBox.transform.position.y + spawnBox.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        spawnBoxBottomPos = spawnBox.transform.position.y - spawnBox.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        float startingX = spawnBox.transform.position.x;
        float startingY = Random.Range(spawnBoxBottomPos, spawnBoxTopPos);
        float startingZ = spawnBox.transform.position.z;


        var startPoint = new Vector3(startingX, startingY, startingZ);
        GameObject newEnemy = null;
        switch (enemy)
        {
            case EnemyType.Normal:
                newEnemy = Instantiate(normalEnemyPrefab, startPoint, Quaternion.identity);
                break;
            case EnemyType.Fast:
                newEnemy = Instantiate(fastEnemyPrefab, startPoint, Quaternion.identity);
                break;
            case EnemyType.Tanking:
                newEnemy = Instantiate(tankingEnemyPrefab, startPoint, Quaternion.identity);
                break;
            case EnemyType.MiniBoss:
                newEnemy = Instantiate(MiniBossEnemyPrefab, startPoint, Quaternion.identity);
                break;
            case EnemyType.Multiple:
                newEnemy = Instantiate(multipleEnemyPrefab, startPoint, Quaternion.identity);
                break;
            case EnemyType.Shield:
                newEnemy = Instantiate(shieldEnemyPrefab, startPoint, Quaternion.identity);
                break;
        }
        if (newEnemy == null)
        {
            return;
        }

        newEnemy.GetComponent<BaseEnemy>().Initialize(this.path, mainCamera, UI_Parent, 0, path[0].transform.position);
    }
}

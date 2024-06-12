using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn1 : BaseSpawn
{
    public WayPoint[] path;
    public override void Spawn(EnemyType enemy)
    {

        var startingX = this.path[0].transform.position.x;
        var startingY = this.path[0].transform.position.y;
        var startingZ = this.path[0].transform.position.z;
        //Debug.Log("path : " + path[0]);
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
        //StaticValues.GetInstance().livingEnemyCount += 1;
        newEnemy.GetComponent<BaseEnemy>().Initialize(this.path, mainCamera, UI_Parent, 0, path[0].transform.position);
    }
}

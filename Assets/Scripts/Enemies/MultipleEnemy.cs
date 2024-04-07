using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 숙제
// 4월 7일 4시에 수업!! 
// 1. Multiple Enemy 작동시키기 (다이나믹한 느낌 꼭 있게끔)
// 2. Enemy 하나만 더 만들기 (마지막 enemy) -> 도전적인 것을 해도 무관 (같이 해결해나가면 되니깐)
public class MultipleEnemy : BaseEnemy
{
    public GameObject childEnemyPrefab;
    public int spawnEnemyCount;

    Quaternion spawnRotation;
    void Start()
    {
        
    }

    void Update()
    {
        this.MoveToNextWayPoint();
        if(this.isDie)
        {
            // for (int i = 0; i <= spawnEnemyCount; i++)
            // {
            //     var thisEnemyRotation = this.transform.rotation;
            //     spawnRotation = Quaternion.Euler(0, 0, thisEnemyRotation.eulerAngles.z + (360 / spawnEnemyCount));
                
            //     var newEnemy = Instantiate(childEnemyPrefab, this.transform.position, spawnRotation);
            //     // position을 조금 다르게 줘야지 서로 겹치지 않는다!
            //     newEnemy.GetComponent<BaseEnemy>().Initialize(this.path, this.mainCamera, this.canvasUI, this.currentWayPointIndex, this.transform.position);
            //     newEnemy.transform.Translate(1, 0, 0);
            // }
            for (int i = 0; i <= 360; i += 360 / spawnEnemyCount)
            {
                var thisEnemyRotation = this.transform.rotation;
                spawnRotation = Quaternion.Euler(0, 0, thisEnemyRotation.eulerAngles.z + i);
                
                var newEnemy = Instantiate(childEnemyPrefab, this.transform.position, spawnRotation);
                // position을 조금 다르게 줘야지 서로 겹치지 않는다!
                newEnemy.GetComponent<BaseEnemy>().Initialize(this.path, this.mainCamera, this.canvasUI, this.currentWayPointIndex, this.transform.position);
                newEnemy.transform.Translate(0.4f, 0, 0);
            }
            Destroy(this.gameObject);
        }
    }
}

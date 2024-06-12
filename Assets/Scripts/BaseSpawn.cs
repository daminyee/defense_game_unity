using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawn : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas UI_Parent;

    LargeTurretSpace largeTurretSpace;

    public GameObject normalEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankingEnemyPrefab;
    public GameObject MiniBossEnemyPrefab;
    public GameObject multipleEnemyPrefab;
    public GameObject shieldEnemyPrefab;

    public abstract void Spawn(EnemyType enemy);
}

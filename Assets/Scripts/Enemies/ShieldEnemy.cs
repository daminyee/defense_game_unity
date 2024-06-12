using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : BaseEnemy
{
    public GameObject sheildPrefab;
    void Start()
    {
        SetMap();
    }

    // Update is called once per frame
    void Update()
    {
        this.MoveToNextWayPoint();
    }
}

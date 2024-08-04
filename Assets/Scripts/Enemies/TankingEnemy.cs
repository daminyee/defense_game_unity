using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankingEnemy : BaseEnemy
{
    void Start()
    {
        SetMap();
    }

    void Update()
    {
        this.MoveToNextWayPoint();
    }
}

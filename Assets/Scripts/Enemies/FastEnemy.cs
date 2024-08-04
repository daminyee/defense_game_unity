using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : BaseEnemy
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

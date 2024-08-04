using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildEnemy : BaseEnemy
{
    void Start()
    {
        transform.Translate(0.3f, 0, 0);
        SetMap();
        FindWayPoint();
    }

    void Update()
    {
        this.MoveToNextWayPoint();
    }
}

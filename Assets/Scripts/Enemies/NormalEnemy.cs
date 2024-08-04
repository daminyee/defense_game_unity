using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalEnemy : BaseEnemy
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

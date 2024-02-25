using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalEnemy : BaseEnemy
{
    void Start()
    {
        this.Initialize();
    }

    // Update is called once per frame
    void Update()
    {   
        this.MoveToNextWayPoint();
    }

 
}

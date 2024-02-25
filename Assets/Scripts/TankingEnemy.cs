using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankingEnemy : BaseEnemy
{
    // Start is called before the first frame update
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

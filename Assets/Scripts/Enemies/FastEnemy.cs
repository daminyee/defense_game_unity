using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : BaseEnemy
{
    // Start is called before the first frame update    
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

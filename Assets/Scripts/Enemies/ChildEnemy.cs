using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildEnemy : BaseEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(0.3f, 0, 0);
        SetMap();
        this.MoveToNextWayPoint();
        FindWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        this.MoveToNextWayPoint();
    }
}

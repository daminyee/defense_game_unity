using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public float radius;

    void Start()
    {
        this.radius = 0.5f;
    }

    void Update()
    {
        // this.transform.localScale = new Vector3(radius, radius, 1);
        this.GetComponent<CircleCollider2D>().radius = radius; 
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            this.transform.parent.GetComponent<BaseTurret>().AddEnemy(enemy);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            this.transform.parent.GetComponent<BaseTurret>().DeleteEnemy(enemy);
        }
    }
}

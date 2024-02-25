using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public float attackDamage;
    public float slowPower;

    public GameObject slowField;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(6 * Time.deltaTime, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            var newSlowField = Instantiate(slowField, enemy.transform.position, Quaternion.identity);
        }
    }
}

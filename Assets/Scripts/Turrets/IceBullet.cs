using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public float attackDamage;
    public float slowPower;
    public Vector2 shootingOrigin;
    public bool shouldDestroyWhenOutOfRange;

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
            Debug.Log(slowField);
            var newSlowField = Instantiate(slowField, enemy.transform.position, Quaternion.identity);
            newSlowField.GetComponent<SlowField>().Initialize(slowPower, attackDamage);
            
            Destroy(this.gameObject);
        }
    }

    public void Initialize(float attackDamage, Vector2 shootingOrigin, float slowPower, bool shouldDestroyWhenOutOfRange = true)
    {
        this.attackDamage = attackDamage;
        this.shootingOrigin = shootingOrigin;
        this.slowPower = slowPower;
        this.shouldDestroyWhenOutOfRange = shouldDestroyWhenOutOfRange;
    }
}

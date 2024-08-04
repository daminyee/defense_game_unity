using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbit : MonoBehaviour
{
    public GameObject parentEnemy;

    public float speed;

    void Start()
    {
        this.speed = 250f;
    }

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}

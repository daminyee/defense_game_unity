using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbit : MonoBehaviour
{
    public GameObject parentEnemy;
    // Start is called before the first frame update

    public float speed;

    void Start()
    {
        this.speed = 250f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float sheildHp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var bullet = collider.GetComponent<Bullet>();
        var sniperBullet = collider.GetComponent<SniperBullet>();

        if (bullet != null)
        {
            this.sheildHp -= bullet.attackDamage;
            Destroy(collider.gameObject);
        }
        if (sniperBullet != null)
        {
            this.sheildHp -= sniperBullet.attackDamage;
            Destroy(collider.gameObject);
        }
        if (this.sheildHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

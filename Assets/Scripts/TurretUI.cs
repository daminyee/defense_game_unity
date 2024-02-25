using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretUI : MonoBehaviour
{
    public BaseTurret turret;
    void Start()
    {
        
    }

    // void Update()
    // {
        
    // }

    public void Upgrade()
    {
        turret.UpgradeTurret();
    }

    public void DestroyUI() 
    {
        Destroy(gameObject);
    }
}

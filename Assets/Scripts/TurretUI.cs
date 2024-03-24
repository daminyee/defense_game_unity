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
            turret.UpgradeTurret(); // 나중에 아래로 내리기
        if(StaticValues.GetInstance().gold >= turret.upgradePrice)
        {

            StaticValues.GetInstance().gold -= turret.upgradePrice;
            turret.turretSpace.ShowGetGold(turret.upgradePrice,false);
            turret.sellPrice += turret.upgradePrice/2;
        }
    }

    public void Sell()
    {
        //turret.SellTurret();
        turret.SellTurret();
        Destroy(this.gameObject);
    }

    public void DestroyUI() 
    {
        turret.isShowingUI = false;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    public BaseTurret turret;

    public Button upgradeButton;
    public Button sellButton;

    public Text upgradePriceText;
    public Text sellPriceText;
    void Start()
    {

    }

    void Update()
    {
        if (StaticValues.GetInstance().gold < turret.upgradePrice)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }

        upgradePriceText.text = $"cost:{turret.upgradePrice}";
        sellPriceText.text = $"cost:{turret.sellPrice}";
    }

    public void Upgrade()
    {
        turret.UpgradeTurret(); // 나중에 아래로 내리기
        if (StaticValues.GetInstance().gold >= turret.upgradePrice)
        {

            StaticValues.GetInstance().gold -= turret.upgradePrice;
            turret.turretSpace.ShowGetGold(turret.upgradePrice, false);
            turret.sellPrice += turret.upgradePrice / 2;
        }
    }

    public void Sell()
    {
        turret.SellTurret();
        StaticValues.GetInstance().openedTurretUI = null;
        Destroy(this.gameObject);
    }

    public void DestroyUI()
    {
        turret.isShowingUI = false;
        turret.MakeAttackRangeInvisible();
        Destroy(gameObject);
    }
}

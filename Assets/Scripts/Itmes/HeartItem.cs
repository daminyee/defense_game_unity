using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartItem : MonoBehaviour
{
    public int itemPrice;
    public Text priceText;
    void Start()
    {

    }

    void Update()
    {
        priceText.text = itemPrice.ToString();
    }

    public void BuyThisItem()
    {
        StaticValues.GetInstance().gold -= itemPrice;
        StaticValues.GetInstance().hp += 1;
    }
}

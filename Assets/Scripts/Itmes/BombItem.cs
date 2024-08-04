using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombItem : MonoBehaviour
{
    public int itemPrice;
    public Text priceText;
    public GameObject bombPrefab;
    public GameObject bombObject;

    void Start()
    {

    }

    void Update()
    {
        priceText.text = itemPrice.ToString();
    }

    public void BuyThisItem()
    {
        bombObject = Instantiate(bombPrefab, new Vector2(0, 0), Quaternion.identity);
        bombObject.GetComponent<Animator>().SetTrigger("Bomb");
        StartCoroutine(BombAnimation());
    }

    public void Bomb()
    {
        StaticValues.GetInstance().gold -= itemPrice;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy); // 골드 획득 X
            StaticValues.GetInstance().livingEnemyCount -= 1;
        }
        Destroy(bombObject);
    }

    IEnumerator BombAnimation()
    {
        var m_CurrentClipInfo = bombObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        Debug.Log(m_CurrentClipInfo);
        //Access the current length of the clip
        var m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
        yield return new WaitForSeconds(m_CurrentClipLength);
        Bomb();
    }
}

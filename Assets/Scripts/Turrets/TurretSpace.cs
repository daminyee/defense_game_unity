using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpace : MonoBehaviour
{
    public GameObject putOnEffect;
    public bool isInstalled = false;

    public BaseTurret installedTurret = null;

    public Vector2 spriteBounds = new Vector2();

    private int originalSortingOrder = 0;
    public int rowIndex;
    public int columnIndex;

    void Start()
    {
        originalSortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder;
        this.spriteBounds.x = this.transform.GetComponent<SpriteRenderer>().bounds.size.x;
        this.spriteBounds.y = this.transform.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {

    }

    public void ShowGetGold(int usedGold, bool getGold)
    {
        // create new text UI element
        // Debug.Log("showusedgold");
        GameObject usedGoldText = new GameObject("UsedGoldText");
        usedGoldText.transform.SetParent(installedTurret.canvasUI.transform, false);
        usedGoldText.AddComponent<RectTransform>();
        usedGoldText.AddComponent<CanvasRenderer>();
        usedGoldText.AddComponent<UnityEngine.UI.Text>();
        if (getGold)
        {
            usedGoldText.GetComponent<UnityEngine.UI.Text>().text = "+" + usedGold;
            usedGoldText.GetComponent<UnityEngine.UI.Text>().color = Color.green;
        }
        else
        {
            usedGoldText.GetComponent<UnityEngine.UI.Text>().text = "-" + usedGold;
            usedGoldText.GetComponent<UnityEngine.UI.Text>().color = Color.red;
        }
        usedGoldText.GetComponent<UnityEngine.UI.Text>().fontSize = 80;
        usedGoldText.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleCenter;
        // change rect transform size
        usedGoldText.GetComponent<RectTransform>().sizeDelta = new Vector2(230, 100);
        usedGoldText.GetComponent<UnityEngine.UI.Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        // set position
        Vector3 screenPoint = installedTurret.mainCamera.WorldToScreenPoint(this.transform.position);
        RectTransform rectTransform = usedGoldText.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(installedTurret.canvasUI.GetComponent<RectTransform>(), screenPoint, installedTurret.canvasUI.worldCamera, out localPoint);
        rectTransform.anchoredPosition = localPoint;

        // usedGoldText.GetComponent<RectTransform>().transform.position = textPos;
        // localPoint.x = 0; // 해당 버튼 위치에 따라서 조정할 필요가 있다
        // localPoint.y = 0;

        StartCoroutine(GoUpAndDestroy(usedGoldText));
    }

    public IEnumerator GoUpAndDestroy(GameObject usedGoldText)
    {
        float time = 0;
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            usedGoldText.transform.position += new Vector3(0, 0.008f, 0);
            yield return null;
        }
        Destroy(usedGoldText);
    }
    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     var draggingTurret = collider.GetComponent<BaseTurret>();

    //     if(draggingTurret != null && !this.isInstalled)
    //     {
    //         HighlightSpace();
    //         //draggingTurret.isHighlightingAnyTurretSpace = true;
    //     }
    // }

    // void OnTriggerStay2D(Collider2D collider)
    // {

    // }

    // void OnTriggerExit2D(Collider2D collider)
    // {
    //     var draggingTurret = collider.GetComponent<BaseTurret>();

    //     if(draggingTurret != null)
    //     {
    //         UnhighlightSpace();
    //         //draggingTurret.isHighlightingAnyTurretSpace = false;
    //     }
    // }

    public void HighlightSpace()
    {
        if (isInstalled) return;
        this.putOnEffect.SetActive(true);
        this.GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrder + 1;
    }
    public void UnhighlightSpace()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrder;
        this.putOnEffect.SetActive(false);
    }

    public void TurnOnIsTrigger()
    {
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    public void TurnOffIsTrigger()
    {
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpace : MonoBehaviour
{
    public GameObject putOnEffect;
    public bool isInstalled = false;

    public BaseTurret installedTurret = null;

    public Vector2 spriteBounds = new Vector2();

    void Start()
    {
        this.spriteBounds.x = this.transform.GetComponent<SpriteRenderer>().bounds.size.x;
        this.spriteBounds.y = this.transform.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        
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
        if(isInstalled) return;
        this.putOnEffect.SetActive(true);
    }

    public void UnhighlightSpace()
    {
        this.putOnEffect.SetActive(false);
    }
}

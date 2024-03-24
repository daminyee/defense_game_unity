using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretOption : ClickDetector
{
    public GameObject draggingTurretObject = null;
    public GameObject turretPrefab;
    public Canvas canvasUI;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0;
        if (draggingTurretObject == null)
        {
            draggingTurretObject = Instantiate(turretPrefab, worldPoint, Quaternion.identity);
            draggingTurretObject.GetComponent<BaseTurret>().Initialize(true, this.mainCamera, this.canvasUI);
        }
        else
        {
            draggingTurretObject.transform.position = worldPoint;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0;
        draggingTurretObject.transform.position = worldPoint;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        var draggingTurret = draggingTurretObject.GetComponent<BaseTurret>();

        if(this.draggingTurretObject != null)
        {   
            if(!draggingTurret.isInValidSpace || (draggingTurret.turretSpace != null && draggingTurret.turretSpace.isInstalled))
            {
                Destroy(draggingTurretObject);
            } else 
            {
                if(StaticValues.GetInstance().gold < draggingTurret.price)
                {
                    Destroy(draggingTurretObject);
                } else {
                    
                    draggingTurret.InstallTurret();
                }
            }
        }
        draggingTurretObject = null;
    }

    private Vector3 ScreenToWorldPoint(Vector2 screenPoint)
    {
        return mainCamera.ScreenToWorldPoint(screenPoint);
    }


}

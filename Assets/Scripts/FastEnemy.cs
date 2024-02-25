using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : BaseEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        this.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        this.MoveToNextWayPoint();


        // 만약에 UI가 아닌 object를 클릭하게 만드려면
        // 무조건 Update문 안에 GetMouseButtonDown을 통해서 감지를 해야하고
        // 감지하는 것도 Collider2D를 통해서 mouse의 지점이 
        // collider2D안에 있는지를 통해서 마우스가 클릭된 것처럼 "억지로" 만들어야 한다.
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (this.GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                // Debug.Log("Fast Enemy Clicked");
            }
        }
    }
}

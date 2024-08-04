using UnityEngine;
using UnityEngine.EventSystems;

// 숙제: 7월 28일까지
// 1. 코드를 정리해서 보자. (코드를 읽기 쉽게 정리하자)
//  필요없는 것은 없애는 작업
// 2. 코드를 정리하면서, Game1 Game2 Game3 어떻게 발전시켰는지를 서술하자. PPT
//  Game1 난제: ClickDetector를 이용해서 클릭 이벤트를 받아서 처리하는 방법을 배웠다.
//  길찾기를 중심적으로 설명하기 바람
public abstract class ClickDetector : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Begin");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("Dragging");
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("Drag Ended");
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Mouse Enter");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Mouse Exit");
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("Mouse Up");
    }
}
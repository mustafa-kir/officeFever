using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ScreenTouchController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image pivotImage;
    private Vector2 _touchPosition;
    
    public Vector2 Direction { get; private set; }
    public void OnDrag(PointerEventData eventData)
    {
        var delta = eventData.position - _touchPosition;
        Direction = delta.normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
    }

   
   
}

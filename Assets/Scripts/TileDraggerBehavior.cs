using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

public class TileDraggerBehavior : MonoBehaviour
{
    [HideInInspector] public int DirectionX;
    [HideInInspector] public int DirectionY;

    [HideInInspector] public UnityEvent<TileDraggerBehavior> onDragEnded = new UnityEvent<TileDraggerBehavior>();
    Vector2 dragStartPosition;
    bool dragging = false;
    bool canStartDrag = true;
    
    private void OnMouseDown()
    {
        if (dragging || !canStartDrag) return;
        dragging = true;
        dragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }
    public void setDragEnable(bool val)
    {
        this.canStartDrag = val;
    }

    private void OnMouseUp()
    {
        dragging = false;
        
    }
    private void OnMouseDrag()
    {
        if (!dragging) return;
        Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2? dir = AngleCalculator.DirectionVectorToAxis(dragStartPosition - currentPos);
        if (dir.HasValue)
        {
            this.DirectionX = (int)dir.Value.x;
            this.DirectionY = (int)dir.Value.y;
            dragging = false;
            onDragEnded.Invoke(this);
        }
    }
    
}

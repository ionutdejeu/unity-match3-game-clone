using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

public class DraggerBehavior : MonoBehaviour
{
    [SerializeField] public int DirectionX;
    [SerializeField] public int DirectionY;

    public UnityEvent<DraggerBehavior> onDragEnded = new UnityEvent<DraggerBehavior>();
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
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    
}

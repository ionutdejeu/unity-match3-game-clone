using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

public class DraggerBehavior : MonoBehaviour
{
    public UnityEvent<DraggerBehavior> onDraggStart = new UnityEvent<DraggerBehavior>();
    Vector2 dragStartPosition;
    
    private void OnMouseDown()
    {
        dragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
    }
    private void OnMouseDrag()
    {
        Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(dragStartPosition, currentPos);
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

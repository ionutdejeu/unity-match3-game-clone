using System;
using UnityEngine;

public class TileGraphicsContentView : MonoBehaviour
{
    
    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer dotSpriteRender;
    public SpriteRenderer selectionSpriteRender;

    private void Start()
    {
        selectionSpriteRender = this.GetComponent<SpriteRenderer>();
    }
}

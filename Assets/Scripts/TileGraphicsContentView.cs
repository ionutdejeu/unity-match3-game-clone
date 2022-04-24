using System;
using UnityEngine;

public class TileGraphicsContentView : MonoBehaviour
{
    
    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer dotSpriteRender;
    public SpriteRenderer selectionSpriteRender;




    private void Start()
    {
        //selectionSpriteRender = this.GetComponent<SpriteRenderer>();
    }
    public void hidrate(TileProperties props)
    {
        dotSpriteRender.color = props.type.backgrounColor;
    }
    public void setIsMatched(TileProperties props)
    {
        if(props.isMatched)
            selectionSpriteRender.color = Color.green;
        else
            selectionSpriteRender.color = Color.white;
    }
}

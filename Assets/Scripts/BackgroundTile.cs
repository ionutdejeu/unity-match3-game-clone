using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public DotsScriptableObject[] dots;
    public SpriteRenderer dotSpriteRender;
    DotsScriptableObject selectedDot;
    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialise()
    {
        int randDot = Random.Range(0, dots.Length);
        this.selectedDot = dots[randDot];
        Debug.Log(selectedDot.backgrounColor);
        this.dotSpriteRender.color = selectedDot.backgrounColor;
    }
}

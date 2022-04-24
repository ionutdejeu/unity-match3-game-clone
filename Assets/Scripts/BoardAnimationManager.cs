using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardAnimationManager : MonoBehaviour
{

    List<SwipeTileAnimation> swipeAnimationsInProgress;
    List<MoveTileWithinBoard> moveTilesAnimationsInProgress;

    private void Awake()
    {
        swipeAnimationsInProgress = new List<SwipeTileAnimation>();
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

using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TileAnimator : MonoBehaviour
{
    [HideInInspector] public TileGraphicsContentView view;
    [HideInInspector] public UnityEvent OnSwipeAnimationCompled = new UnityEvent();
    [HideInInspector] public UnityEvent OnEnterStageAnimationCompleted = new UnityEvent();

    bool isAnimating = false;
    TileProperties animationEndState;

    
    public void AnimateDropIn(TileProperties props)
    {

    }

    public void AnimateSwap(TileProperties current,TileProperties target)
    {
        isAnimating = true;
        animationEndState = target;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating)
            return;
        this.transform.position = AngleCalculator.GetAnimatedPositionBetweenTilePosition(this.transform.position, animationEndState.worldPos, 0.1f);
        if (Vector3.Magnitude(animationEndState.worldPos - this.transform.position) < 0.05f)
        {
            this.isAnimating = false;
            //snap the tile to position
            this.transform.position = animationEndState.worldPos;

            this.OnSwipeAnimationCompled.Invoke();
        }
    }
}

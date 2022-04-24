using UnityEngine;
using System.Collections;
using UnityEngine.Events;



public class TileAnimator : MonoBehaviour
{
    [HideInInspector] public TileGraphicsContentView view;
    [HideInInspector] public UnityEvent OnSwipeAnimationCompled = new UnityEvent();
    [HideInInspector] public UnityEvent OnEnterStageAnimationCompleted = new UnityEvent();
    [HideInInspector] public UnityEvent<TileSwapEventArgs> OnSwapAnimationCompled = new UnityEvent<TileSwapEventArgs>();

    bool isAnimating = false;
    TileProperties animationEndState;
    Vector3 endStateWorldPos;
    TileSwapEventArgs args;

    
    public void AnimateDropIn(TileProperties props)
    {

    }

    public void AnimateMoveToBoardTile()
    {

    }

    public void AnimateSwap(TileProperties current,TileProperties target)
    {
        isAnimating = true;
        animationEndState = target;
    }
    public void AnimateSwap(TileSwapEventArgs args)
    {
        isAnimating = true;
        this.args = args;
        animationEndState = args.targetPlaceholder.tileRef.Props;
        endStateWorldPos = Vector3.zero + args.targetPlaceholder.tileRef.Props.worldPos;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isAnimating)
            return;
        this.transform.position = AngleCalculator.GetAnimatedPositionBetweenTilePosition(this.transform.position, endStateWorldPos, 0.1f);
        if (Vector3.Magnitude(endStateWorldPos - this.transform.position) < 0.05f)
        {
            this.isAnimating = false;
            //snap the tile to position
            this.transform.position = endStateWorldPos;

            this.OnSwipeAnimationCompled.Invoke();
            if (this.args != null)
                this.OnSwapAnimationCompled.Invoke(this.args);
        }
    }
}

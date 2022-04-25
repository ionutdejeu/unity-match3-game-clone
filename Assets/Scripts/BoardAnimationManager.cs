using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class BoardAnimationManager : MonoBehaviour
{

    List<SwipeTileAnimation> swipeAnimations = new List<SwipeTileAnimation>();
    List<MoveTileWithinBoard> moveTileWithinBoard = new List<MoveTileWithinBoard>();
    List<MoveTileFromOutsideToSlot> moveTilesFromOutsideBoard = new List<MoveTileFromOutsideToSlot>();
    List<MoveTileWithinBoard> completedAnimsMovingTilesWithinBoard = new List<MoveTileWithinBoard>();
    List<MoveTileFromOutsideToSlot> completedAnimsMoveTilesFromOutsideBoard = new List<MoveTileFromOutsideToSlot>();
    bool firedEventOnMoveTilesFromOutsideCompleted = false;
    bool firedEventOnMoveTilesWithinBoardCompleted = false;


    [HideInInspector] public UnityEvent<SwipeTileAnimation> OnSwapAnimationCompleted = new UnityEvent<SwipeTileAnimation>();
    [HideInInspector] public UnityEvent<List<MoveTileWithinBoard>> OnMoveTilesWithinBoardCompleted = new UnityEvent<List<MoveTileWithinBoard>>();
    [HideInInspector] public UnityEvent<List<MoveTileFromOutsideToSlot>> OnMoveTilesFromOutsideCompleted = new UnityEvent<List<MoveTileFromOutsideToSlot>>();

    public void AddSwipeAnim(SwipeTileAnimation anim)
    {
        anim.isActive = true;
        swipeAnimations.Add(anim);
    }
    // Update is called once per frame
    void Update()
    {
        if (!firedEventOnMoveTilesWithinBoardCompleted && moveTileWithinBoard.Count > 0)
        {
            foreach (var anim in moveTileWithinBoard)
            {
                if (!anim.isActive)
                {
                    completedAnimsMovingTilesWithinBoard.Add(anim);
                }
                anim.Update(1f);
            }
            foreach (var item in completedAnimsMovingTilesWithinBoard)
            {
                if (moveTileWithinBoard.Contains(item))
                {
                    moveTileWithinBoard.Remove(item);
                }
            }
        }
        else
        {
            firedEventOnMoveTilesWithinBoardCompleted = true;
            OnMoveTilesWithinBoardCompleted.Invoke(completedAnimsMovingTilesWithinBoard);
        }
        if (swipeAnimations.Count > 0)
        {
            List<SwipeTileAnimation> animsToRemove = new List<SwipeTileAnimation>();
            foreach (var anim in swipeAnimations)
            {
                if (!anim.isActive)
                {
                    OnSwapAnimationCompleted.Invoke(anim);
                    animsToRemove.Add(anim);
                }
                anim.Update();
            }
            foreach (var item in animsToRemove)
            {
                swipeAnimations.Remove(item);
            }

        }
        if (!firedEventOnMoveTilesFromOutsideCompleted && moveTilesFromOutsideBoard.Count > 0)
        {
            foreach (var anim in moveTilesFromOutsideBoard)
            {
                if (!anim.isActive)
                {
                    completedAnimsMoveTilesFromOutsideBoard.Add(anim);
                }
                anim.Update(1f);
            }
            foreach (var item in completedAnimsMoveTilesFromOutsideBoard)
            {
                if (moveTilesFromOutsideBoard.Contains(item))
                {
                    moveTilesFromOutsideBoard.Remove(item);
                }
            }
        }
        else
        {
            firedEventOnMoveTilesFromOutsideCompleted = true;
            OnMoveTilesFromOutsideCompleted.Invoke(completedAnimsMoveTilesFromOutsideBoard);
        }
        
    }


    
    

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class BoardAnimationManager : MonoBehaviour
{

    List<SwipeTileAnimation> swipeAnimationList = new List<SwipeTileAnimation>();
    List<MoveTileWithinBoard> moveTileWithinBoardList = new List<MoveTileWithinBoard>();
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
        swipeAnimationList.Add(anim);
    }

    public void AddAnimsMoveTilesWithinBoard(List<MoveTileWithinBoard> anims)
    {
        moveTileWithinBoardList.AddRange(anims);
    }

    public void AddAnimMoveTilesFromOutsideBoard(List<MoveTileFromOutsideToSlot> anims)
    {
        moveTilesFromOutsideBoard.AddRange(anims);
    }
    // Update is called once per frame
    void Update()
    {
        if (!firedEventOnMoveTilesWithinBoardCompleted && moveTileWithinBoardList.Count > 0)
        {
            foreach (var anim in moveTileWithinBoardList)
            {
                if (!anim.isActive)
                {
                    completedAnimsMovingTilesWithinBoard.Add(anim);
                }
                anim.Update(1f);
            }
            foreach (var item in completedAnimsMovingTilesWithinBoard)
            {
                if (moveTileWithinBoardList.Contains(item))
                {
                    moveTileWithinBoardList.Remove(item);
                }
            }
        }
        else
        {
            firedEventOnMoveTilesWithinBoardCompleted = true;
            OnMoveTilesWithinBoardCompleted.Invoke(completedAnimsMovingTilesWithinBoard);
        }
        if (swipeAnimationList.Count > 0)
        {
            List<SwipeTileAnimation> animsToRemove = new List<SwipeTileAnimation>();
            foreach (var anim in swipeAnimationList)
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
                swipeAnimationList.Remove(item);
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

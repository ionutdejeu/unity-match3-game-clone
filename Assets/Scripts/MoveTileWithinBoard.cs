using System;
using UnityEngine;

[Serializable]
public class MoveTileWithinBoard
{
    
    public BoardPlaceholderProperties sourceSlot;
    public BoardPlaceholderProperties targetSlot;
    public TileManager tileToMove;
    public bool isActive = false;
    private bool animationComplete = false;

    public static MoveTileWithinBoard CreateInstance(BoardPlaceholderProperties from, BoardPlaceholderProperties to)
    {
        var movingAnim = new MoveTileWithinBoard();
        movingAnim.sourceSlot = from;
        movingAnim.targetSlot = to;
        movingAnim.tileToMove = from.tileRef;
        return movingAnim;
    }
    public MoveTileWithinBoard()
    {

    }
    void tryUpdateTile()
    {
        Vector3 newAPos = AngleCalculator.GetAnimatedPositionBetweenTilePosition(tileToMove.transform.position, targetSlot.worldPos, 0.1f);
        tileToMove.transform.position = newAPos;
        if (Vector3.Magnitude(targetSlot.worldPos - tileToMove.transform.position) < 0.05f)
        {
            //snap the tile to position
            tileToMove.transform.position = targetSlot.worldPos;
            animationComplete = true;

        }

    }

    public void Update(float deltaTime)
    {
        if (!animationComplete)
            tryUpdateTile();
        
    }
}

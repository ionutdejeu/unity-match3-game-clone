using System;
using UnityEngine;

[Serializable]
public class MoveTileFromOutsideToSlot
{
    
    public BoardPlaceholderProperties targetBoardSlot;
    public TileManager tileToMove;
    public bool isActive = true;

    public static MoveTileFromOutsideToSlot CreateInstance(BoardPlaceholderProperties targetSlot, TileManager tileToMove)
    {
        var movingAnim = new MoveTileFromOutsideToSlot();
        movingAnim.targetBoardSlot = targetSlot;
        movingAnim.tileToMove = tileToMove;
        return movingAnim;
    }
    public MoveTileFromOutsideToSlot()
    {

    }

    void tryUpdateTile()
    {
        Vector3 newAPos = AngleCalculator.GetAnimatedPositionBetweenTilePosition(tileToMove.transform.position, targetBoardSlot.worldPos, 0.1f);
        tileToMove.transform.position = newAPos;
        if (Vector3.Magnitude(targetBoardSlot.worldPos - tileToMove.transform.position) < 0.05f)
        {
            //snap the tile to position
            tileToMove.transform.position = targetBoardSlot.worldPos;
            isActive = false;

        }

    }

    public void Update(float deltaTime)
    {
        if (isActive)
            tryUpdateTile();

    }
}

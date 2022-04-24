using System;
using UnityEngine;

[Serializable]
public class SwipeTileAnimation
{
    
    public BoardPlaceholderProperties tileAPlaceholder;
    public BoardPlaceholderProperties tileBPlaceholder;
    public TileManager tileAManager;
    public TileManager tileBManager;
    
    public bool isActive = false;
    private bool tileACompleted = false;
    private bool tileBCompleted = false;

    public static SwipeTileAnimation CreateInstance(BoardPlaceholderProperties from, BoardPlaceholderProperties to)
    {
        var newInstance = new SwipeTileAnimation();
        newInstance.tileAPlaceholder = from;
        newInstance.tileBPlaceholder = to;
        newInstance.tileAManager = from.tileRef;
        newInstance.tileBManager = to.tileRef;
        


        return newInstance;
    }
    public SwipeTileAnimation()
    {
    }

    void tryUpdateATile()
    {
        Vector3 newAPos = AngleCalculator.GetAnimatedPositionBetweenTilePosition(tileAManager.transform.position, tileBPlaceholder.worldPos, 0.1f);
        tileAManager.transform.position = newAPos;
        if (Vector3.Magnitude(tileBPlaceholder.worldPos - tileAManager.transform.position) < 0.05f)
        {
            //snap the tile to position
            tileAManager.transform.position = tileBPlaceholder.worldPos;
            tileACompleted = true;
        
        }
        
    }

    void tryUpdateBTile()
    {
        Vector3 newAPos = AngleCalculator.GetAnimatedPositionBetweenTilePosition(tileBManager.transform.position, tileAPlaceholder.worldPos, 0.1f);
        tileBManager.transform.position = newAPos;
        if (Vector3.Magnitude(tileAPlaceholder.worldPos - tileBManager.transform.position) < 0.05f)
        {
            //snap the tile to position
            tileBManager.transform.position = tileBPlaceholder.worldPos;
            tileACompleted = true;

        }
    }
    public void Update()
    {
        if (!tileACompleted)
            tryUpdateATile();
        if (!tileBCompleted)
            tryUpdateBTile();
        if (tileBCompleted && tileBCompleted)
            isActive = false;

    }
}

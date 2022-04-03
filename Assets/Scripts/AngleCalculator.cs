using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AngleCalculator
{


    
    public static Tile GetBoardTileBasedOnSwipeDirection(Tile[,] tiles, Tile sourceTile, Vector2 swipeDirection, int boardWidht,int boardHeight)
    {
        Debug.Log($"Source index x:{sourceTile.IndexX} and y:{sourceTile.IndexY}");

        int targetIndexX = sourceTile.IndexX - ((int)swipeDirection.x);
        int targetIndexY = sourceTile.IndexY - ((int)swipeDirection.y);
        Debug.Log($"swipe direction x:{swipeDirection.x} and y:{swipeDirection.y}");
         
        if (targetIndexX >= boardWidht) return sourceTile;
        if (targetIndexY >= boardHeight) return sourceTile;
        if (targetIndexY < 0) return sourceTile;
        if (targetIndexX < 0) return sourceTile;

        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (tiles[i, j].IndexX == targetIndexX && tiles[i, j].IndexY == targetIndexY)
                {
                    Debug.Log($"found target tile with x:{targetIndexX} and y:{targetIndexY} at index: {i},{j} ");
                    return tiles[i, j];
                }
            }
        }
        return sourceTile;
                
    }
    public static Vector2? DirectionVectorToAxis(Vector3 dir,float threshold = 0.8f)
    {
        dir.Normalize();
        float h = Vector3.Dot(Vector3.up, dir);
        float v = Vector3.Dot(Vector3.right, dir);
        float thresholdSq = threshold * threshold;
        float hSq = h * h;
        float vSq = v * v;

        if (Mathf.Abs(h)< threshold && Mathf.Abs(v) < threshold)
        {
            return null;
        }

        if (Mathf.Abs(h) > Mathf.Abs(v))
        {
            return Vector2.up * Mathf.Sign(h);
            // return new Vector2(1f*Mathf.Sign(h),0);
        }
        return Vector2.right * Mathf.Sign(v);

        //return new Vector2(0f,1f * Mathf.Sign(v));

    }
    public static Vector2 GetMovementDirection(Vector3 dir)
    {
        dir.Normalize();
        float v = Vector3.Dot(Vector3.up, dir);
        float h = Vector3.Dot(Vector3.right, dir);
        return new Vector2(v, h);
    }
    public static float ComputeAngleBetweenVectors(Vector3 v1,Vector2 v2)
    {
        return Mathf.Atan2(v1.y - v2.y, v1.x - v2.x) * Mathf.Rad2Deg;
    }

    public static Vector3 GetAnimatedPositionBetweenTilePosition(Vector3 fromTilePos,Vector3 toTilePos,float t)
    {
        return Vector3.Lerp(fromTilePos, toTilePos, t);
    }

}

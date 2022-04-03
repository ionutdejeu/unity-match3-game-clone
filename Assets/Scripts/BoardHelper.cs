using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardHelper
{
    public static DotsScriptableObject[,] GenerateInitialBoard(int boardWidth, int boardHeight)
    {
        DotsScriptableObject[,] allTiles = new DotsScriptableObject[boardWidth,boardHeight];


        return allTiles;
    }



    public static List<List<Tile>> FindMatchingTiles(Tile[,] board,int boardWidht,int boardHeight)
    {
        List<List<Tile>> matchingTiles = new List<List<Tile>>();
        Dictionary<DotsScriptableObject, List<Tile>> mappedTiles = new Dictionary<DotsScriptableObject, List<Tile>>();

        // recompute the tiles based on index;
        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (!mappedTiles.ContainsKey(board[i, j].selectedDot))
                {
                    mappedTiles.Add(board[i, j].selectedDot, new List<Tile>());
                }
                mappedTiles[board[i, j].selectedDot].Add(board[i, j]);
            }
        }
        foreach (KeyValuePair<DotsScriptableObject, List<Tile>> entry in mappedTiles)
        {
            Tile[,] mappedTilesOfOneTypes = new Tile[boardWidht, boardHeight];
            for (int i = 0; i < boardWidht; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    mappedTilesOfOneTypes[i, j] = null;
                }
            }
            foreach (Tile t in entry.Value)
            {
                mappedTilesOfOneTypes[t.IndexX, t.IndexY] = t;
            }

            foreach (Tile t in entry.Value)
            {
                List<Tile> matchesInRow = GetCountOfMatchingTilesInRow(mappedTilesOfOneTypes, boardWidht, boardHeight, t.IndexX, t.IndexY);
                if (matchesInRow.Count>0)
                {
                    matchingTiles.Add(matchesInRow);
                }
                List<Tile> matchesInCol = GetCountOfMatchingTilesInColumn(mappedTilesOfOneTypes, boardWidht, boardHeight, t.IndexX, t.IndexY);
                if (matchesInCol.Count > 0)
                {
                    matchingTiles.Add(matchesInCol);
                }
            }
        }
        return matchingTiles;
    }

    public static List<Tile> GetCountOfMatchingTilesInRow(Tile[,] matchingTileMatrix,int boardWithd,int boardHeight, int sourceX, int sourceY)
    {
        List<Tile> matchFound = new List<Tile>();
        int leftIndex = Math.Clamp(sourceX - 1, 0, boardWithd-1);
        int left2Index = Math.Clamp(sourceX - 2, 0, boardWithd-1);
        int rightIndex = Math.Clamp(sourceX + 1, 0, boardWithd-1);
        int right2Index = Math.Clamp(sourceX + 1, 0, boardWithd-1);
        bool foundLeft1 = matchingTileMatrix[leftIndex, sourceY] != null && leftIndex != sourceX;
        bool foundLeft2 = matchingTileMatrix[left2Index, sourceY] != null && left2Index != sourceX;
        bool foundRight1 = matchingTileMatrix[left2Index, sourceY] != null && rightIndex != sourceX;
        bool foundRight2 = matchingTileMatrix[left2Index, sourceY] != null && right2Index != sourceX;

        bool matchedLeft = foundLeft2 && foundLeft1;
        bool matchedRight = foundRight1 && foundRight2;
        bool matchedBothSides = !foundLeft2 && foundLeft1 && foundRight1 && !foundRight2;
        bool bonusMatchedAllSides = foundLeft2 && foundLeft1 && foundRight1 && foundRight2;

        if (bonusMatchedAllSides || matchedLeft)
        {
            Debug.Log($"found match on x:{sourceX - 2},{sourceX - 1}");
            if(foundLeft2)
            matchFound.Add(matchingTileMatrix[left2Index, sourceY]);
            if(foundLeft1)
            matchFound.Add(matchingTileMatrix[leftIndex, sourceY]);
        }

        if (bonusMatchedAllSides || matchedRight)
        {
            Debug.Log($"found match on x:{sourceX + 1},{sourceX + 1}");
            if(foundRight1)
            matchFound.Add(matchingTileMatrix[rightIndex, sourceY]);
            if(foundRight2)
            matchFound.Add(matchingTileMatrix[right2Index, sourceY]);
        }
        if (matchedBothSides)
        {
            if(foundLeft1)
            matchFound.Add(matchingTileMatrix[leftIndex, sourceY]);
            if(foundRight1)
            matchFound.Add(matchingTileMatrix[rightIndex, sourceY]);
        }
        return matchFound;
    }

    public static List<Tile> GetCountOfMatchingTilesInColumn(Tile[,] matchingTileMatrix, int boardWithd, int boardHeight, int sourceX, int sourceY)
    {
        List<Tile> matchFound = new List<Tile>();
        int upIndex = Math.Clamp(sourceY + 1, 0, boardHeight-1);
        int up2Index = Math.Clamp(sourceY + 2, 0, boardHeight-1);
        int downIndex = Math.Clamp(sourceY - 1, 0, boardHeight-1);
        int down2Index = Math.Clamp(sourceY - 2, 0, boardHeight-1);
        Debug.Log($"checking on x:{sourceX},y:{upIndex}");
        bool foundUp1 = matchingTileMatrix[sourceX, upIndex] != null && upIndex != sourceY;
        bool foundUp2 = matchingTileMatrix[sourceX, up2Index] != null && up2Index != sourceY;
        bool foundDown1 = matchingTileMatrix[sourceX, downIndex] != null && downIndex != sourceY;
        bool foundDown2 = matchingTileMatrix[sourceX, down2Index] != null && down2Index != sourceY;

        bool matchedUp = foundUp1 && foundUp2;
        bool matchedDown = foundDown1 && foundDown2;
        bool matchedBothSides = !foundDown2 && foundDown1 && foundUp1 && !foundDown2;
        bool bonusMatchedAllSides = foundDown2 && foundDown1 && foundUp1 && foundDown2;

        if (bonusMatchedAllSides || matchedDown)
        {
            if(foundDown2)
            matchFound.Add(matchingTileMatrix[sourceX,down2Index]);
            if(foundDown1)
            matchFound.Add(matchingTileMatrix[sourceX,downIndex]);
        }

        if (bonusMatchedAllSides || matchedUp)
        {
            if(foundUp1)
            matchFound.Add(matchingTileMatrix[sourceX,upIndex]);
            if(foundUp2)
            matchFound.Add(matchingTileMatrix[sourceX,up2Index]);
        }
        if (matchedBothSides)
        {
            matchFound.Add(matchingTileMatrix[sourceX,downIndex]);
            matchFound.Add(matchingTileMatrix[sourceX,upIndex]);
        }
        return matchFound;
    }


}

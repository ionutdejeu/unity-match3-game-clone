using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardHelper
{
    public static BoardPlaceholderProperties[,] GenerateInitialBoard(int boardWidth, int boardHeight)
    {
        BoardPlaceholderProperties[,] allTilesContent = new BoardPlaceholderProperties[boardWidth,boardHeight];
        // recompute the tiles based on index;
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                allTilesContent[i, j].x = i;
                allTilesContent[i, j].y = j;
                allTilesContent[i, j].worldPos = new Vector3(i,j,-1);
            }
        }
        
        return allTilesContent;
    }


    public static void AssingRandomTilesToBoardPlaceholders(
        List<TileTypeScriptableObject> possibleTypes,
        BoardPlaceholderProperties[,] boardPlaceholders,
        int boardWidth,
        int boardHeight,
        GameObject boardTilePrefab,
        GameObject parent)
    {
        int randDot = UnityEngine.Random.Range(0, possibleTypes.Count);
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                GameObject tileInstance = GameObject.Instantiate(boardTilePrefab, boardPlaceholders[i, j].worldPos, Quaternion.identity);
                boardPlaceholders[i, j].tileRef = tileInstance.GetComponent<TileManager>();
                tileInstance.transform.parent = parent.transform;
                tileInstance.name = "item (" + i + ", " + j + ")";
                TileProperties tileProps = new TileProperties();
                tileProps.x = boardPlaceholders[i, j].x;
                tileProps.y = boardPlaceholders[i, j].y;
                tileProps.worldPos = boardPlaceholders[i, j].worldPos;
                
            }
        }
    }

    public static List<List<TileManager>> FindMatchingTiles(BoardPlaceholderProperties[,] board, int boardWidht, int boardHeight)
    {
        List<List<TileManager>> matchingTiles = new List<List<TileManager>>();
        Dictionary<string, List<TileManager>> mappedTiles = new Dictionary<string, List<TileManager>>();
        // recompute the tiles based on index;
        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (!mappedTiles.ContainsKey(board[i, j].tileRef.Props.type.name))
                {
                    mappedTiles.Add(board[i, j].tileRef.Props.type.name, new List<TileManager>());
                }
                mappedTiles[board[i, j].tileRef.Props.type.name].Add(board[i, j].tileRef);
            }
        }
        foreach (KeyValuePair<string, List<TileManager>> entry in mappedTiles)
        {
            TileManager[,] mappedTilesOfOneTypes = new TileManager[boardWidht, boardHeight];
            for (int i = 0; i < boardWidht; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    mappedTilesOfOneTypes[i, j] = null;
                }
            }
            foreach (TileManager t in entry.Value)
            {
                mappedTilesOfOneTypes[t.IndexX, t.IndexY] = t;
            }

            foreach (TileManager t in entry.Value)
            {
                List<TileManager> matchesInRow = GetCountOfMatchingTilesInRow(mappedTilesOfOneTypes, boardWidht, boardHeight, t.IndexX, t.IndexY);
                if (matchesInRow.Count > 0)
                {
                    matchingTiles.Add(matchesInRow);
                }
                List<TileManager> matchesInCol = GetCountOfMatchingTilesInColumn(mappedTilesOfOneTypes, boardWidht, boardHeight, t.IndexX, t.IndexY);
                if (matchesInCol.Count > 0)
                {
                    matchingTiles.Add(matchesInCol);
                }
            }
        }
        return matchingTiles;
    }

    public static List<List<TileManager>> FindMatchingTiles(TileManager[,] board,int boardWidht,int boardHeight)
    {
        List<List<TileManager>> matchingTiles = new List<List<TileManager>>();
        Dictionary<TileTypeScriptableObject, List<TileManager>> mappedTiles = new Dictionary<TileTypeScriptableObject, List<TileManager>>();

        // recompute the tiles based on index;
        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (!mappedTiles.ContainsKey(board[i, j].selectedDot))
                {
                    mappedTiles.Add(board[i, j].selectedDot, new List<TileManager>());
                }
                mappedTiles[board[i, j].selectedDot].Add(board[i, j]);
            }
        }
        foreach (KeyValuePair<TileTypeScriptableObject, List<TileManager>> entry in mappedTiles)
        {
            TileManager[,] mappedTilesOfOneTypes = new TileManager[boardWidht, boardHeight];
            for (int i = 0; i < boardWidht; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    mappedTilesOfOneTypes[i, j] = null;
                }
            }
            foreach (TileManager t in entry.Value)
            {
                mappedTilesOfOneTypes[t.IndexX, t.IndexY] = t;
            }

            foreach (TileManager t in entry.Value)
            {
                List<TileManager> matchesInRow = GetCountOfMatchingTilesInRow(mappedTilesOfOneTypes, boardWidht, boardHeight, t.IndexX, t.IndexY);
                if (matchesInRow.Count>0)
                {
                    matchingTiles.Add(matchesInRow);
                }
                List<TileManager> matchesInCol = GetCountOfMatchingTilesInColumn(mappedTilesOfOneTypes, boardWidht, boardHeight, t.IndexX, t.IndexY);
                if (matchesInCol.Count > 0)
                {
                    matchingTiles.Add(matchesInCol);
                }
            }
        }
        return matchingTiles;
    }

    public static List<TileManager> GetCountOfMatchingTilesInRow(TileManager[,] matchingTileMatrix,int boardWithd,int boardHeight, int sourceX, int sourceY)
    {
        List<TileManager> matchFound = new List<TileManager>();
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

    public static List<TileManager> GetCountOfMatchingTilesInColumn(TileManager[,] matchingTileMatrix, int boardWithd, int boardHeight, int sourceX, int sourceY)
    {
        List<TileManager> matchFound = new List<TileManager>();
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

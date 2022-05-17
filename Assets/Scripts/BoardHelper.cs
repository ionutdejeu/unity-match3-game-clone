using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
                allTilesContent[i, j] = new BoardPlaceholderProperties();
                allTilesContent[i, j].x = i;
                allTilesContent[i, j].y = j;
                allTilesContent[i, j].worldPos = new Vector3(i,j,-1);
            }
        }
        
        return allTilesContent;
    }

    public static BoardPlaceholderProperties[,] GenerateInitialBoardWithoutMatches(int boardWidth, int boardHeight)
    {
        BoardPlaceholderProperties[,] allTilesContent = new BoardPlaceholderProperties[boardWidth, boardHeight];
        // recompute the tiles based on index;
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                allTilesContent[i, j] = new BoardPlaceholderProperties();
                allTilesContent[i, j].x = i;
                allTilesContent[i, j].y = j;
                allTilesContent[i, j].worldPos = new Vector3(i, j, -1);
            }
        }

        return allTilesContent;
    }
    public static TileTypeScriptableObject[,] FindBoardSolutionWithoutMatches(
         TileTypeScriptableObject[] possibleTypes,
        int boardWidth,
        int boardHeight
        )
    {
        TileTypeScriptableObject[,] possibleBoardSolution;
        do
        {
            possibleBoardSolution = TryFindBoardSolutionWithoutMatches(possibleTypes, boardWidth, boardHeight);

        } while (possibleBoardSolution == null);
        return possibleBoardSolution;
    }
    public static TileTypeScriptableObject[,]? TryFindBoardSolutionWithoutMatches(
        TileTypeScriptableObject[] possibleTypes,
        int boardWidth,
        int boardHeight)
    {
        TileTypeScriptableObject[,] boardTypes = new TileTypeScriptableObject[boardHeight, boardHeight];

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                List<TileTypeScriptableObject> matchingTiles = new List<TileTypeScriptableObject>();
                int findRandomMatchRetries = 0;
                do
                {
                    int randDot = UnityEngine.Random.Range(0, possibleTypes.Length);
                    boardTypes[i, j] = possibleTypes[randDot];
                    matchingTiles = FindMatchingTypes(boardTypes, boardWidth, boardHeight, i, j);
                    findRandomMatchRetries++;
                } while (findRandomMatchRetries < 3 && matchingTiles.Count > 0);
                if (findRandomMatchRetries >= 3)
                {
                    return null;
                }

            }

        }

        return boardTypes;
    }
    public static void AssingTilesToBoardPlaceholdersWithoutMatches(
        TileTypeScriptableObject[,] boardTypes,
        BoardPlaceholderProperties[,] boardPlaceholders,
        int boardWidth,
        int boardHeight,
        GameObject boardTilePrefab,
        GameObject parent,
        UnityAction<TileSwipeEventArgs> onTileDraggedHandler
        //UnityAction<TileManager> onSwipeAnimationCompletedHandler
        )
    {
        bool boardGenerationCompleted = false;
        while (!boardGenerationCompleted)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    

                    GameObject tileInstance = GameObject.Instantiate(boardTilePrefab, boardPlaceholders[i, j].worldPos, Quaternion.identity);
                    TileManager tm = tileInstance.GetComponent<TileManager>();
                    tileInstance.transform.parent = parent.transform;
                    tileInstance.name = "item (" + i + ", " + j + ")";
                    TileProperties tileProps = new TileProperties();
                    tileProps.x = boardPlaceholders[i, j].x;
                    tileProps.y = boardPlaceholders[i, j].y;
                    tileProps.worldPos = boardPlaceholders[i, j].worldPos;
                    boardPlaceholders[i, j].tileRef = tm;
                    tileProps.type = boardTypes[i, j];
                    tm.setTileContentProperties(tileProps);
                    tm.OnTileDragged.AddListener(onTileDraggedHandler);
                    //tm.OnTileSwipeAnimationCompled.AddListener(onSwipeAnimationCompletedHandler);
                    //tm.OnTileSwapCompleted.AddListener(onTileSwapCompletedEventHandler);

                }
            }
            boardGenerationCompleted = true;

        }
    }


    public static void AssingRandomTilesToBoardPlaceholders(
        TileTypeScriptableObject[] possibleTypes,
        BoardPlaceholderProperties[,] boardPlaceholders,
        int boardWidth,
        int boardHeight,
        GameObject boardTilePrefab,
        GameObject parent,
        UnityAction<TileSwipeEventArgs> onTileDraggedHandler,
        UnityAction<TileManager> onSwipeAnimationCompletedHandler,
        UnityAction<TileSwapEventArgs> onTileSwapCompletedEventHandler)
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                int randDot = UnityEngine.Random.Range(0, possibleTypes.Length);

                GameObject tileInstance = GameObject.Instantiate(boardTilePrefab, boardPlaceholders[i, j].worldPos, Quaternion.identity);
                TileManager tm = tileInstance.GetComponent<TileManager>();
                tileInstance.transform.parent = parent.transform;
                tileInstance.name = "item (" + i + ", " + j + ")";
                TileProperties tileProps = new TileProperties();
                tileProps.x = boardPlaceholders[i, j].x;
                tileProps.y = boardPlaceholders[i, j].y;
                tileProps.worldPos = boardPlaceholders[i, j].worldPos;
                tileProps.type = possibleTypes[randDot];
                tm.setTileContentProperties(tileProps);
                tm.OnTileDragged.AddListener(onTileDraggedHandler);
                tm.OnTileSwipeAnimationCompled.AddListener(onSwipeAnimationCompletedHandler);
                tm.OnTileSwapCompleted.AddListener(onTileSwapCompletedEventHandler);
                boardPlaceholders[i, j].tileRef = tm;
            }
        }
    }
    public static List<List<TileManager>> FindMatchingTiles2(BoardPlaceholderProperties[,] board, int boardWidht, int boardHeight)
    {
        List<List<TileManager>> matchingTiles = new List<List<TileManager>>();
        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                List<TileManager> matchesFound = FindMatchesAroundTheSpecifiedIndex(board, boardWidht, boardHeight, i, j);
                if (matchesFound.Count > 0)
                {
                    matchesFound.Add(board[i,j].tileRef);
                    matchingTiles.Add(matchesFound);
                }

            }
        }
        return matchingTiles;
    }
    public static List<List<BoardPlaceholderProperties>> FindMatchingPlaceholders(BoardPlaceholderProperties[,] board, int boardWidht, int boardHeight)
    {
        List<List<BoardPlaceholderProperties>> matchingTiles = new List<List<BoardPlaceholderProperties>>();
        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (board[i, j].tileRef)
                {
                    List<BoardPlaceholderProperties> matchesFound = FindMatchingPlaceholderAroundTheSpecifiedIndex(board, boardWidht, boardHeight, i, j);
                    if (matchesFound.Count > 0)
                    {
                        matchesFound.Add(board[i, j]);
                        matchingTiles.Add(matchesFound);
                    }
                }
            }

        }
        return matchingTiles;
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
                mappedTilesOfOneTypes[t.Props.x,t.Props.y] = t;
            }

            foreach (TileManager t in entry.Value)
            {
                List<TileManager> matchesInRow = GetCountOfMatchingTilesInRow(mappedTilesOfOneTypes, boardWidht, boardHeight, t.Props.x, t.Props.y);
                if (matchesInRow.Count > 0)
                {
                    matchingTiles.Add(matchesInRow);
                }
                List<TileManager> matchesInCol = GetCountOfMatchingTilesInColumn(mappedTilesOfOneTypes, boardWidht, boardHeight, t.Props.x, t.Props.y);
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
        Dictionary<string, List<TileManager>> mappedTiles = new Dictionary<string, List<TileManager>>();

        // recompute the tiles based on index;
        for (int i = 0; i < boardWidht; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (!mappedTiles.ContainsKey(board[i, j].Props.type.name))
                {
                    mappedTiles.Add(board[i, j].Props.type.name, new List<TileManager>());
                }
                mappedTiles[board[i, j].Props.type.name].Add(board[i, j]);
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
                mappedTilesOfOneTypes[t.Props.x, t.Props.y] = t;
            }

            foreach (TileManager t in entry.Value)
            {
                List<TileManager> matchesInRow = GetCountOfMatchingTilesInRow(mappedTilesOfOneTypes, boardWidht, boardHeight, t.Props.x, t.Props.y);
                if (matchesInRow.Count>0)
                {
                    matchingTiles.Add(matchesInRow);
                }
                List<TileManager> matchesInCol = GetCountOfMatchingTilesInColumn(mappedTilesOfOneTypes, boardWidht, boardHeight, t.Props.x, t.Props.y);
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
        if (matchFound.Count > 0)
        {
            matchFound.Add(matchingTileMatrix[sourceX, sourceY]);
        }
        return matchFound;
    }
    public static void DestroyMatchedTiles(List<BoardPlaceholderProperties> matchedTiles)
    { 
        foreach(BoardPlaceholderProperties t in matchedTiles)
        {
            if (t.tileRef)
            {
                t.tileRef.animateDestroy();
                GameObject.Destroy(t.tileRef.gameObject);
                t.tileRef = null;
            }
        }
    }
    public BoardPlaceholderProperties[] CreateTileSpawnPositions(int count)
    {

        BoardPlaceholderProperties[] spawnPositions = new BoardPlaceholderProperties[count];
        // make sure to spawn them outside of the screen

        for (int i = 0; i < count; i++)
        {
            BoardPlaceholderProperties sp  = new BoardPlaceholderProperties();
            sp.worldPos = new Vector3(i, 10, 0);
            sp.x = i;
            sp.y = 0;
            spawnPositions[i] = sp;
        }

        return spawnPositions;
    }
    public static void ReplenishBoard(BoardPlaceholderProperties[,] boardTiles)
    {

    }
    
    
    public static BoardPlaceholderProperties FindTargetToMoveTheTileAfterDestroy(BoardPlaceholderProperties[,] board, int boardWidth, int boardHeight, int x, int y)
    {
        //look downwards for the specified tile
        for (int i = y - 1; i >= 0; i--)
        {
            if (board[x, i].tileRef != null && board[x, i].tileRef.isActiveAndEnabled)
            {
                return board[x, i];
            }
        }
        return null;
    }
    public static List<TileTypeScriptableObject> FindMatchingTypes(TileTypeScriptableObject[,] boardTileTypes,int boardWidth,int boardHeight,int x, int y)
    {
        List<TileTypeScriptableObject> matchFound = new List<TileTypeScriptableObject>();
        // loop starting from the central point all directions
        // loop in the left of the specified index
        string sourceTileType = boardTileTypes[x, y].name;

        List<TileTypeScriptableObject> leftMatches = new List<TileTypeScriptableObject>();
        for (int i = x - 1; i >= 0; i--)
        {
            if (boardTileTypes[i, y] == null ||
                boardTileTypes[i, y].name != sourceTileType)
            {
                break;
            }
            leftMatches.Add(boardTileTypes[i, y]);
        }
        if (leftMatches.Count > 1)
        {
            matchFound.AddRange(leftMatches);
        }

        // loop thorught all the tiles in the right of the source tile
        List<TileTypeScriptableObject> rightMatches = new List<TileTypeScriptableObject>();
        for (int i = x + 1; i < boardWidth; i++)
        {
            if (boardTileTypes[i, y] == null ||
                boardTileTypes[i, y].name != sourceTileType)
            {
                break;
            }
            rightMatches.Add(boardTileTypes[i, y]);
        }
        if (rightMatches.Count > 1)
        {
            matchFound.AddRange(rightMatches);
        }

        // loop thorught all the tiles in the right of the source tile
        List<TileTypeScriptableObject> downMatches = new List<TileTypeScriptableObject>();
        for (int i = y - 1; i >= 0; i--)
        {
            if (boardTileTypes[x, i] == null ||
                boardTileTypes[x, i].name != sourceTileType)
            {
                break;
            }
            downMatches.Add(boardTileTypes[x, i]);
        }
        if (downMatches.Count > 1)
        {
            matchFound.AddRange(downMatches);
        }

        // loop thorught all the tiles in the right of the source tile 
        List<TileTypeScriptableObject> upMatches = new List<TileTypeScriptableObject>();

        for (int i = y + 1; i < boardHeight; i++)
        {
            if (boardTileTypes[x, i] == null ||
                boardTileTypes[x, i].name != sourceTileType)
            {
                break;
            }
            upMatches.Add(boardTileTypes[x, i]);
        }
        if (upMatches.Count > 1)
        {
            matchFound.AddRange(upMatches);
        }



        return matchFound;
    }
    public static List<BoardPlaceholderProperties> FindMatchingPlaceholderAroundTheSpecifiedIndex(BoardPlaceholderProperties[,] board, int boardWithd, int boardHeight, int x, int y)
    {
        List<BoardPlaceholderProperties> matchFound = new List<BoardPlaceholderProperties>();
        // loop starting from the central point all directions
        // loop in the left of the specified index

        string sourceTileType = board[x, y].tileRef.Props.type.name;

        List<BoardPlaceholderProperties> leftMatches = new List<BoardPlaceholderProperties>();
        for (int i = x - 1; i >= 0; i--)
        {
            if (board[i, y].tileRef == null ||
                board[i, y].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            leftMatches.Add(board[i, y]);
        }
        if (leftMatches.Count > 1)
        {
            matchFound.AddRange(leftMatches);
        }

        // loop thorught all the tiles in the right of the source tile
        List<BoardPlaceholderProperties> rightMatches = new List<BoardPlaceholderProperties>();
        for (int i = x + 1; i < boardWithd; i++)
        {
            if (board[i, y].tileRef == null ||
                board[i, y].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            rightMatches.Add(board[i, y]);
        }
        if (rightMatches.Count > 1)
        {
            matchFound.AddRange(rightMatches);
        }

        // loop thorught all the tiles in the right of the source tile
        List<BoardPlaceholderProperties> downMatches = new List<BoardPlaceholderProperties>();
        for (int i = y - 1; i >= 0; i--)
        {
            if (board[x, i].tileRef == null ||
                board[x, i].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            downMatches.Add(board[x, i]);
        }
        if (downMatches.Count > 1)
        {
            matchFound.AddRange(downMatches);
        }

        // loop thorught all the tiles in the right of the source tile 
        List<BoardPlaceholderProperties> upMatches = new List<BoardPlaceholderProperties>();

        for (int i = y + 1; i < boardHeight; i++)
        {
            if (board[x, i].tileRef == null ||
                board[x, i].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            upMatches.Add(board[x, i]);
        }
        if (upMatches.Count > 1)
        {
            matchFound.AddRange(upMatches);
        }
        return matchFound;
    }
    public static List<TileManager> FindMatchesAroundTheSpecifiedIndex(BoardPlaceholderProperties[,] board, int boardWithd, int boardHeight,int x,int y)
    {
        List<TileManager> matchFound = new List<TileManager>();
        // loop starting from the central point all directions
        // loop in the left of the specified index
        string sourceTileType = board[x, y].tileRef.Props.type.name;

        List<TileManager> leftMatches = new List<TileManager>();
        for(int i = x-1; i >= 0; i--)
        {
            if (board[i, y].tileRef == null ||
                board[i, y].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            leftMatches.Add(board[i, y].tileRef);
        }
        if (leftMatches.Count > 1)
        {
            matchFound.AddRange(leftMatches);
        }

        // loop thorught all the tiles in the right of the source tile
        List<TileManager> rightMatches = new List<TileManager>();
        for (int i = x + 1; i < boardWithd; i++)
        {
            if (board[i, y].tileRef == null ||
                board[i, y].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            rightMatches.Add(board[i, y].tileRef);
        }
        if (rightMatches.Count > 1)
        {
            matchFound.AddRange(rightMatches);
        }

        // loop thorught all the tiles in the right of the source tile
        List<TileManager> downMatches = new List<TileManager>();
        for (int i = y - 1; i >=0; i--)
        {
            if (board[x, i].tileRef == null ||
                board[x, i].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            downMatches.Add(board[x, i].tileRef);
        }
        if (downMatches.Count > 1)
        {
            matchFound.AddRange(downMatches);
        }

        // loop thorught all the tiles in the right of the source tile 
        List<TileManager> upMatches = new List<TileManager>();

        for (int i = y + 1; i < boardHeight; i++)
        {
            if (board[x, i].tileRef == null ||
                board[x, i].tileRef.Props.type.name != sourceTileType)
            {
                break;
            }
            upMatches.Add(board[x, i].tileRef);
        }
        if (upMatches.Count > 1)
        {
            matchFound.AddRange(upMatches);
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
        bool up1 = matchingTileMatrix[sourceX, upIndex] != null && upIndex != sourceY;
        bool up2 = matchingTileMatrix[sourceX, up2Index] != null && up2Index != sourceY;
        bool down1 = matchingTileMatrix[sourceX, downIndex] != null && downIndex != sourceY;
        bool down2 = matchingTileMatrix[sourceX, down2Index] != null && down2Index != sourceY;

        bool matchedUp = up1 && up2;
        bool matchedDown = down1 && down2;
        bool matchedBothSides = !down2 && down1 && up1 && !up2;
        bool bonusMatchedAllSides = down2 && down1 && up2 && up1;

        if (bonusMatchedAllSides || matchedDown)
        {
            if(down2)
            matchFound.Add(matchingTileMatrix[sourceX,down2Index]);
            if(down1)
            matchFound.Add(matchingTileMatrix[sourceX,downIndex]);
        }

        if (bonusMatchedAllSides || matchedUp)
        {
            if(up1)
            matchFound.Add(matchingTileMatrix[sourceX,upIndex]);
            if(up2)
            matchFound.Add(matchingTileMatrix[sourceX,up2Index]);
        }
        if (matchedBothSides)
        {
            matchFound.Add(matchingTileMatrix[sourceX,downIndex]);
            matchFound.Add(matchingTileMatrix[sourceX,upIndex]);
        }
        if (matchFound.Count > 0)
        {
            matchFound.Add(matchingTileMatrix[sourceX, sourceY]);
        }
        return matchFound;
    }


}

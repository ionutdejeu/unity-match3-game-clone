using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{

    [SerializeField] GameObject backgroundTilePrefab;
    [SerializeField] GameObject tilePrefab;

    [SerializeField] int width;
    [SerializeField] int height;
    public TileTypeScriptableObject[] dots;

 
    private TileManager[,] tiles;
    private BoardPlaceholderProperties[,] boardSlots;
    
    // Start is called before the first frame update
    void Start()
    {
        setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTileDraggHandler(TileSwipeEventArgs args)
    {
        TileManager target = AngleCalculator.GetBoardTileBasedOnSwipeDirection(boardSlots, args.tile, new Vector2(args.dirX, args.dirY), width, height);
        Debug.Log(target.name);
        if (target != args.tile)
        {
            target.StartMovementTowards(args.tile.Props);
            args.tile.StartMovementTowards(target.Props);
        }
    }

    void OnSwipeAnimationCompleted(TileManager sourceTile)
    {
        List<TileManager> allTiles = new List<TileManager>();

        // recompute the tiles based on index;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allTiles.Add(tiles[i,j]);
               // tiles[i,j].setIsMatched(false);
            }

        }
        for(int i = 0; i < allTiles.Count; i++)
        {
            TileManager t = allTiles[i];
            //tiles[t.IndexX, t.IndexY] = t;
        }

        List<List<TileManager>> matchingTiles = BoardHelper.FindMatchingTiles(tiles, width, height);
        Debug.Log(matchingTiles.Count);

        foreach(List<TileManager> matches in matchingTiles)
        {
            foreach(TileManager t in matches)
            {
                //if(t!=null)
                //t.setIsMatched(true);
            }
        }
    }

    void setup()
    {
        backgroundTiles = new BackgroundTile[width, height];
        tiles = new TileManager[width, height];
        allTiles = new TileManager[width, height];
        boardSlots = BoardHelper.GenerateInitialBoard(width, height);
        BoardHelper.AssingRandomTilesToBoardPlaceholders(dots,
            boardSlots,
            width,
            height,
            tilePrefab,
            this.gameObject,
            this.OnTileDraggHandler,
            this.OnSwipeAnimationCompleted);
        
        //for (int i = 0; i < width; i++)
        //{
        //    for(int j = 0;j< height; j++)
        //    {
        //        Vector2 tempPos = new Vector2(i, j);
        //        GameObject backTile = GameObject.Instantiate(backgroundTilePrefab, tempPos, Quaternion.identity);
        //        backgroundTiles[i,j] = backTile.GetComponent<BackgroundTile>();
        //        backTile.transform.parent = this.transform;
        //        backTile.name = "background (" + i + ", " + j + ")";
        //        Vector3 tilePos = new Vector3(i, j,-1);
        //
        //        GameObject tObj = GameObject.Instantiate(tilePrefab, tilePos, Quaternion.identity);
        //        TileManager t = tObj.GetComponent<TileManager>();
        //        t.IndexX = i;
        //        t.IndexY = j;
        //        tObj.transform.parent = this.transform;
        //        tObj.name = "tile (" + i + ", " + j + ")";
        //        t.OnTileDragged.AddListener(OnTileDraggHandler);
        //        tiles[i, j] = t;
        //        allTiles[i, j] = t;
        //        t.OnTileSwipeAnimationCompled.AddListener(this.OnAnimationComplete);
        //    }
        //}
    }
}

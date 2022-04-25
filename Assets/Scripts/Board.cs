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

    [SerializeField] private BoardPlaceholderProperties[,] boardSlots;
    BoardAnimationManager animManager;

    private void Awake()
    {
        animManager = GetComponent<BoardAnimationManager>();
        animManager.OnSwapAnimationCompleted.AddListener(this.OnTileSwapAnimationCompleted);

    }
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
        Debug.Log("found target tile:"+ target.name);
        BoardPlaceholderProperties sourcePlaceholder = AngleCalculator.GetBoardPlaceholder(boardSlots, args.tile);
        BoardPlaceholderProperties targetPlaceholder = AngleCalculator.GetBoardPlaceholder(boardSlots, target);

        //TileSwapEventArgs sourceSwapEventsArgs = new TileSwapEventArgs()
        //{
        //    sourcePlaceholder = targetPlaceholder,
        //    targetPlaceholder = sourcePlaceholder,
        //    sourceTile = target,
        //    targetTile = args.tile,
        //    refreshMatches = false
        //};
        //
        //TileSwapEventArgs targetSwapEventsArgs = new TileSwapEventArgs()
        //{
        //    sourcePlaceholder = sourcePlaceholder,
        //    targetPlaceholder = targetPlaceholder,
        //    sourceTile = args.tile,
        //    targetTile = target,
        //    refreshMatches = true
        //};
        //
        //
        //if (target != args.tile)
        //{
        //    target.swapTilePlaceholder(sourceSwapEventsArgs);
        //    args.tile.swapTilePlaceholder(targetSwapEventsArgs);
        //}
        animManager.AddSwipeAnim(SwipeTileAnimation.CreateInstance(sourcePlaceholder, targetPlaceholder));
        
    }
    void OnTileSwapAnimationCompleted(SwipeTileAnimation args)
    {
        Debug.Log("new Swipe Anim Completed ref :" + args.tileAManager.Props);
        args.tileAPlaceholder.tileRef = args.tileBManager;
        args.tileBPlaceholder.tileRef = args.tileAManager;
        args.tileAManager.setNewPlaceholder(args.tileBPlaceholder);
        args.tileBManager.setNewPlaceholder(args.tileAPlaceholder);
        //if (args.refreshMatches)
        UpdateMatchedTiles();

    }

    void OnTileSwapCompleted(TileSwapEventArgs args)
    {
        Debug.Log("targetpalceholder ref :"+ args.targetPlaceholder.tileRef.Props);
        //this handler is triggered twice for each tile completed
        args.sourcePlaceholder.tileRef = args.targetTile;
        //update the indexes of targetTile
       // args.targetTile.setNewPlaceholder(args.sourcePlaceholder);
        args.sourceTile.setNewPlaceholder(args.targetPlaceholder);

        //if (args.refreshMatches)
        UpdateMatchedTiles();

    }
    
    void UpdateMatchedTiles()
    {
        // recompute the tiles based on index;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                boardSlots[i, j].tileRef.setIsMatched(false);
            }

        }
        List<List<TileManager>> matchingTiles = BoardHelper.FindMatchingTiles2(boardSlots, width, height);

        foreach (List<TileManager> matches in matchingTiles)
        {
            foreach (TileManager t in matches)
            {
                if (t != null)
                    t.setIsMatched(true);
            }
        }
        foreach (List<TileManager> matches in matchingTiles)
        {
           
            //BoardHelper.DestroyMatchedTiles(matches);

        }

    }

    void OnSwipeAnimationCompleted(TileManager sourceTile)
    {

       
        
    }

    void setup()
    {

        boardSlots = BoardHelper.GenerateInitialBoard(width, height);
        TileTypeScriptableObject[,] boardTileTypes = BoardHelper.FindBoardSolutionWithoutMatches(dots, width, height);
        BoardHelper.AssingTilesToBoardPlaceholdersWithoutMatches(boardTileTypes,
            boardSlots,
            width,
            height,
            tilePrefab,
            this.gameObject,
            this.OnTileDraggHandler,
            this.OnSwipeAnimationCompleted,
            this.OnTileSwapCompleted);
    }
}

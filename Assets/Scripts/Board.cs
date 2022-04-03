using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] GameObject backgroundTilePrefab;
    [SerializeField] GameObject tilePrefab;

    [SerializeField] int width;
    [SerializeField] int height;
    private BackgroundTile[,] backgroundTiles;
    private Tile[,] tiles;
    private Tile[,] allTiles;
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

        Debug.Log($"x:{args.dirX},y:{args.dirY},source X:{args.tile.IndexX},,source Y:{args.tile.IndexY}");
        Tile target = AngleCalculator.GetBoardTileBasedOnSwipeDirection(tiles, args.tile, new Vector2(args.dirX, args.dirY), width, height);
        if (target != args.tile)
        {
            target.StartMovementTowards(args.tile);
            args.tile.StartMovementTowards(target);
        }
    }

    void OnAnimationComplete(Tile sourceTile)
    {
        List<Tile> allTiles = new List<Tile>();

        // recompute the tiles based on index;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allTiles.Add(tiles[i,j]);
                tiles[i,j].setIsMatched(false);
            }

        }
        for(int i = 0; i < allTiles.Count; i++)
        {
            Tile t = allTiles[i];
            tiles[t.IndexX, t.IndexY] = t;
        }

        List<List<Tile>> matchingTiles = BoardHelper.FindMatchingTiles(tiles, width, height);
        Debug.Log(matchingTiles.Count);

        foreach(List<Tile> matches in matchingTiles)
        {
            foreach(Tile t in matches)
            {
                if(t!=null)
                t.setIsMatched(true);
            }
        }
    }

    void setup()
    {
        backgroundTiles = new BackgroundTile[width, height];
        tiles = new Tile[width, height];
        allTiles = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for(int j = 0;j< height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject backTile = GameObject.Instantiate(backgroundTilePrefab, tempPos, Quaternion.identity);
                backgroundTiles[i,j] = backTile.GetComponent<BackgroundTile>();
                backTile.transform.parent = this.transform;
                backTile.name = "background (" + i + ", " + j + ")";
                Vector3 tilePos = new Vector3(i, j,-1);

                GameObject tObj = GameObject.Instantiate(tilePrefab, tilePos, Quaternion.identity);
                Tile t = tObj.GetComponent<Tile>();
                t.IndexX = i;
                t.IndexY = j;
                tObj.transform.parent = this.transform;
                tObj.name = "tile (" + i + ", " + j + ")";
                t.OnTileDragged.AddListener(OnTileDraggHandler);
                tiles[i, j] = t;
                allTiles[i, j] = t;
                t.OnTileSwipeAnimationCompled.AddListener(this.OnAnimationComplete);
            }
        }
    }
}

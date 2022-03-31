using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] GameObject tilePrefab;
    [SerializeField] int width;
    [SerializeField] int height;
    private BackgroundTile[,] tiles;
    // Start is called before the first frame update
    void Start()
    {
        setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setup()
    {
        tiles = new BackgroundTile[width, height];
        for(int i = 0; i < width; i++)
        {
            for(int j = 0;j< height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject o = GameObject.Instantiate(tilePrefab, tempPos, Quaternion.identity);
                tiles[i,j] = o.GetComponent<BackgroundTile>();
                o.transform.parent = this.transform;
                o.name = "(" + i + ", " + j + ")";
            }
        }
    }
}

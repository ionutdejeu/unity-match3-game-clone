using UnityEngine;
using System.Collections;
using UnityEditor.Events;
using UnityEngine.Events;


[System.Serializable]
public class TileSwipeEvent : UnityEvent<TileSwipeEventArgs> { }

public class TileSwipeEventArgs
{
    public Tile tile;
    public int dirX;
    public int dirY;

    public TileSwipeEventArgs(Tile t,int dirX,int dirY)
    {
        this.tile = t;
        this.dirX = dirX;
        this.dirY = dirY;
    }
}

public class Tile : MonoBehaviour
{
    public int IndexX;
    public int IndexY;
    [HideInInspector] public DraggerBehavior dragger;
    public DotsScriptableObject[] dots;
    public SpriteRenderer dotSpriteRender;
    public SpriteRenderer selectionSpriteRender;

    [HideInInspector] public DotsScriptableObject selectedDot;

    [HideInInspector] public TileSwipeEvent OnTileDragged = new TileSwipeEvent();
    [HideInInspector] public UnityEvent<Tile> OnTileSwipeAnimationCompled = new UnityEvent<Tile>();

    private Tile targetTile;
    private Vector3 targetTilePosition;
    private int targetTileIndexX;
    private int targetTileIndexY;
    bool animateMovement = false;
    bool _isPartOfMatch = false;
    public void StartMovementTowards(Tile targetTile)
    {
        this.targetTile = targetTile;
        this.targetTilePosition = targetTile.transform.position;
        this.dragger.setDragEnable(false);
        this.targetTile.dragger.setDragEnable(false);
        this.animateMovement = true;
        targetTileIndexX = targetTile.IndexX;
        targetTileIndexY = targetTile.IndexY;
    }

    public void StartMovementTowardsIndex(int indexX, int indexY)
    {
        
    }

    public void Start()
    {
        dragger = this.gameObject.GetComponent<DraggerBehavior>();
        dragger.onDragEnded.AddListener(this.OnCurrentTileDragStart);
        selectionSpriteRender = this.GetComponent<SpriteRenderer>();
        Initialise();

    }
    public void setIsMatched(bool isSelected)
    {
        _isPartOfMatch = isSelected;
        if(_isPartOfMatch)
            selectionSpriteRender.color = Color.green;
        else
            selectionSpriteRender.color = Color.white;


    }
    void OnCurrentTileDragStart(DraggerBehavior d)
    {
        OnTileDragged.Invoke(new TileSwipeEventArgs(this, d.DirectionX, d.DirectionY));
    }

    public void SwapTilePosition()
    { 
        this.IndexX = targetTileIndexX;
        this.IndexY = targetTileIndexY;   
    }

    // Update is called once per frame
    void Update()
    {
        if (this.animateMovement)
        {
            this.transform.position = AngleCalculator.GetAnimatedPositionBetweenTilePosition(this.transform.position, targetTilePosition, 0.1f);
            if (Vector3.Magnitude(targetTilePosition - this.transform.position) < 0.05f)
            {
                this.animateMovement = false;
                this.dragger.setDragEnable(true);
                this.targetTile.dragger.setDragEnable(true);
                //snap the tile to position
                this.transform.position = targetTilePosition;
                this.IndexX = targetTileIndexX;
                this.IndexY = targetTileIndexY;
                Debug.Log($"t moved to idX:{this.IndexX},idY:{this.IndexY}");
                //this.SwapTilePosition();
                this.OnTileSwipeAnimationCompled.Invoke(this);
            }
        }
    }

    void Initialise()
    {
        int randDot = Random.Range(0, dots.Length);
        this.selectedDot = dots[randDot];
        this.dotSpriteRender.color = selectedDot.backgrounColor;
    }

}

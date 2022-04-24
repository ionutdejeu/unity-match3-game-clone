using UnityEngine;
using System.Collections;
using UnityEditor.Events;
using UnityEngine.Events;


[System.Serializable]
public class TileSwipeEvent : UnityEvent<TileSwipeEventArgs> { }

public class TileSwipeEventArgs
{
    public TileManager tile;
    public int dirX;
    public int dirY;

    public TileSwipeEventArgs(TileManager t,int dirX,int dirY)
    {
        this.tile = t;
        this.dirX = dirX;
        this.dirY = dirY;
    }
}

public class TileManager : MonoBehaviour
{
    
    [HideInInspector] public TileDraggerBehavior dragger;
    [HideInInspector] public TileAnimator animator;
    [SerializeField] private TileProperties _props;
    public TileProperties Props { get { return _props; } }
    [HideInInspector] public TileGraphicsContentView view;
    [HideInInspector] public TileTypeScriptableObject selectedDot;
    [HideInInspector] public TileSwipeEvent OnTileDragged = new TileSwipeEvent();
    [HideInInspector] public UnityEvent<TileManager> OnTileSwipeAnimationCompled = new UnityEvent<TileManager>();
    [HideInInspector] public UnityEvent<TileSwapEventArgs> OnTileSwapCompleted = new UnityEvent<TileSwapEventArgs>();


    public void setTileContentProperties(TileProperties new_properties)
    {
        this._props = new_properties;
        // animate the transition
        Debug.Log(_props);
        view.hidrate(new_properties);
    }    


    public void setIsMatched(bool val)
    {
        this._props.isMatched = val;
        view.setIsMatched(this._props);
    }

    public void isMatched(bool value)
    {
        //update ui
        _props.isMatched = value;
        
    }

    public void setNewPlaceholder(BoardPlaceholderProperties newPlaceholder)
    {
        Debug.Log($"old props:" + this._props);
        this._props.x = newPlaceholder.x;
        this._props.y = newPlaceholder.y;
        this._props.worldPos = newPlaceholder.worldPos;
        Debug.Log($"assigned new props:" + this._props);
    }

    public void swapTileWith(TileSwapEventArgs args)
    {
        this.dragger.setDragEnable(false);
        this.animator.AnimateSwap(args);
    }
    public void swapTileWith(TileProperties targetTile)
    {
        this.dragger.setDragEnable(false);
        this.animator.AnimateSwap(this._props, targetTile);
    }
    public void swapTilePlaceholder(TileSwapEventArgs args)
    {
        this.dragger.setDragEnable(false);
        this.animator.AnimateSwap(args);
    }

    public void Awake()
    {
        this.view = this.gameObject.GetComponent<TileGraphicsContentView>();
        this.dragger = this.gameObject.GetComponent<TileDraggerBehavior>();
        this.dragger.onDragEnded.AddListener(this.OnCurrentTileDragStart);
        this.animator = this.gameObject.GetComponent<TileAnimator>();
        this.animator.OnSwipeAnimationCompled.AddListener(OnSwipeAnimationCompled);
        this.animator.OnSwapAnimationCompled.AddListener(this.OnSwapAnimationCompled);
    }

    public void animateDestroy()
    {
        this.gameObject.SetActive(false);
        GameObject.Destroy(this.gameObject, 1f);
    }
    
    void OnCurrentTileDragStart(TileDraggerBehavior d)
    {
        OnTileDragged.Invoke(new TileSwipeEventArgs(this, d.DirectionX, d.DirectionY));
    }

    void OnSwapAnimationCompled(TileSwapEventArgs args)
    {

        OnTileSwapCompleted.Invoke(args);
        this.dragger.setDragEnable(true);
    }
    void OnSwipeAnimationCompled()
    {
        OnTileSwipeAnimationCompled.Invoke(this);
        this.dragger.setDragEnable(true);
    }

}

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
    private TileProperties _props;
    public TileProperties Props {get;private set;}
    [HideInInspector] public TileGraphicsContentView view;
    [HideInInspector] public TileTypeScriptableObject selectedDot;
    [HideInInspector] public TileSwipeEvent OnTileDragged = new TileSwipeEvent();
    [HideInInspector] public UnityEvent<TileManager> OnTileSwipeAnimationCompled = new UnityEvent<TileManager>();


    public void setTileContentProperties(TileProperties new_properties)
    {
        this._props = new_properties;
        // animate the transition
        Debug.Log(view);
        view.hidrate(new_properties);
    }    


    public void isMatched(bool value)
    {
        //update ui
        _props.isMatched = value;
        
    }
    public void StartMovementTowards(TileProperties targetTile)
    {
        this.dragger.setDragEnable(false);
        this.animator.AnimateSwap(this._props, targetTile);
    }


    public void Awake()
    {
        this.view = this.gameObject.GetComponent<TileGraphicsContentView>();
        this.dragger = this.gameObject.GetComponent<TileDraggerBehavior>();
        this.dragger.onDragEnded.AddListener(this.OnCurrentTileDragStart);
        this.animator = this.gameObject.GetComponent<TileAnimator>();
        this.animator.OnSwipeAnimationCompled.AddListener(OnSwipeAnimationCompled);
    }
    
    void OnCurrentTileDragStart(TileDraggerBehavior d)
    {
        OnTileDragged.Invoke(new TileSwipeEventArgs(this, d.DirectionX, d.DirectionY));
    }

    void OnSwipeAnimationCompled()
    {
        OnTileSwipeAnimationCompled.Invoke(this);
        this.dragger.setDragEnable(true);
    }


}

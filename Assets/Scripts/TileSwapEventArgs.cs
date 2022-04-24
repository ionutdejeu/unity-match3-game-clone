using System;
public class TileSwapEventArgs
{
    public BoardPlaceholderProperties sourcePlaceholder;
    public BoardPlaceholderProperties targetPlaceholder;
    public TileManager sourceTile;
    public TileManager targetTile;
    public bool refreshMatches = false;

    public TileSwapEventArgs()
    {
    }
}

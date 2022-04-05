using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BoardPlaceholderProperties
{
    public int x;
    public int y;
    public Vector3 worldPos;
    public TileManager tileRef; 
}

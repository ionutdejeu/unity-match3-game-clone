using System;
using UnityEngine;

[Serializable]
public struct TileProperties
{
    public int x;
    public int y;
    public bool isMatched;
    public Vector3 worldPos;
    public TileTypeScriptableObject type;
}


[Serializable]
public struct TileAnimation
{
    public int isYoyo;
    public Vector3 fromWorldPos;
    public Vector3 toWorldPos;
}

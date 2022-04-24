using System;
using UnityEngine;

[Serializable]
public class TileProperties
{
    public int x;
    public int y;
    public bool isMatched;
    public Vector3 worldPos;
    public TileTypeScriptableObject type;

    public override string ToString()
    {
        return $"{x}:{y}, matched:{isMatched}, type:{type}";
    }
}

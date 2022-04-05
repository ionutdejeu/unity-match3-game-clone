using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TileTypeScriptableObject", menuName = "Tiles")]
public class TileTypeScriptableObject : ScriptableObject
{
    [SerializeField] public string tileType;
    [SerializeField] public Color backgrounColor;
    // Use this for initialization
}

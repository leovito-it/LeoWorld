using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tdata", menuName ="Tile Manager")]
public class TileData : ScriptableObject
{
    public List<Vector2> blockList;
    public Vector2 start_pos = new Vector2(-1,-1);
    public Vector2 end_pos = new Vector2(-1,-1);
}

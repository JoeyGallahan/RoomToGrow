using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    public enum TileType
    {
        NONE,
        FLOWER,
        WEED
    }

    [SerializeField] private TileID tileID;
    [SerializeField] private TileType containingType;
    [SerializeField] private SpriteRenderer containingSprite;
    
    //TileID Stuff
    public TileID GetTileID()
    {
        return tileID;
    }
    public void SetTileID(TileID id)
    {
        tileID = id;
    }
    public void SetTileID(int row, int col)
    {
        tileID = new TileID(row, col);
    }
    public int GetRow()
    {
        return tileID.row;
    }
    public int GetColumn()
    {
        return tileID.col;
    }

    //TileType Stuff
    public TileType GetTileType()
    {
        return containingType;
    }
    public void SetTileType(TileType type)
    {
        containingType = type;
    }

    //Sprite in Tile Stuff
    public Sprite GetSprite()
    {
        return containingSprite.sprite;
    }
    public void SetSprite(Sprite sprite)
    {
        if (sprite != null)
            containingSprite.sprite = sprite;
        else if (containingSprite.sprite != null)
            containingSprite.sprite = null;
    }

    //Where the tile is
    [System.Serializable]
    public struct TileID
    {
        public int row;
        public int col;

        public TileID(int r, int c)
        {
            row = r;
            col = c;
        }
    }
}

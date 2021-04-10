using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileManager
{
    [SerializeField] private List<Tile> allTiles;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 middleTile;
    [SerializeField] private Sprite weedSprite;
    [SerializeField] private Sprite flowerSprite;

    public int numFlowers { get; private set; }

    Dictionary<Tile, Tile.TileType> tilesToChange;
    
/**
 * Accessors
 */
    //Returns all the tiles on the map
    public List<Tile> GetTiles()
    {
        return allTiles;
    }

    //Get a tile at a specific row & column
    private Tile GetTile(int row, int col)
    {
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].GetRow() == row && allTiles[i].GetColumn() == col)
                return allTiles[i];
        }
        return null;
    }

    //Returns the index of a tile at a specific row & column
    public int GetIndexOfTile(int row, int col)
    {
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].GetRow() == row &&
                allTiles[i].GetColumn() == col)
            {
                return i;
            }
        }
        return -1;
    }

    //Returns how many tiles have a weed or flower next to them in any direction, including diagonally
    private Dictionary<Tile.TileType, int> GetNumberOfNeighbors(Tile tile, int numRows, int numCols)
    {
        Dictionary<Tile.TileType, int> neighbors = new Dictionary<Tile.TileType, int>()
        {
            { Tile.TileType.FLOWER, 0},
            { Tile.TileType.WEED, 0}
        };

        int row = tile.GetRow();
        int col = tile.GetColumn();

        for (int r = row - 1; r < row + 2; r++)
        {
            for (int c = col - 1; c < col + 2; c++)
            {
                //If it's not the same row as the tile you're looking at and
                //If it's not going off the map
                if ((r != row || c != col) &&
                     r >= 0 && c >= 0 &&
                     r < numRows && c < numCols)
                {
                    Tile.TileType tileType = GetTile(r, c).GetTileType();
                    if (tileType == Tile.TileType.FLOWER)
                    {
                        neighbors[Tile.TileType.FLOWER]++;
                    }
                    else if (tileType == Tile.TileType.WEED)
                    {
                        neighbors[Tile.TileType.WEED]++;
                    }
                }
            }
        }
        return neighbors;
    }

/**
 * Change all tiles
 */

    private void FindMiddleTile(int numRows, int numCols)
    {
        float middleRowIndex = numRows / 2;
        float middleColIndex = numCols / 2;

        middleTile = new Vector2(middleRowIndex, middleColIndex);
    }

    //Updates all the tiles on the map 
    public void SetTiles(List<Tile> tiles)
    {
        //numFlowers = 0;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (allTiles[i].GetRow() == tiles[i].GetTileID().row &&
                allTiles[i].GetColumn() == tiles[i].GetTileID().col &&
                tiles[i].GetTileType() != allTiles[i].GetTileType())
            {
                switch (tiles[i].GetTileType())
                {
                    case Tile.TileType.FLOWER:
                        PlantFlower(allTiles[i]);
                        break;
                    case Tile.TileType.WEED:
                        SpawnWeed(allTiles[i]);
                        break;
                    case Tile.TileType.NONE:
                        PruneTile(allTiles[i]);
                        break;
                    default:
                        Debug.Log("How did you do this?");
                        break;
                }
            }
        }
    }

    //Adds all the tiles to the map
    public void SpawnTiles(int numRows, int numCols)
    {
        RemoveTiles();

        FindMiddleTile(numRows, numCols);

        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                //Find the distance between the middle tile and the current tile being spawned
                int diffC = 0;
                int diffR = 0;
                if (c != (int)middleTile.x)
                {
                    diffC = (int)middleTile.x - c;    
                }
                if (r != (int)middleTile.y)
                {
                    diffR = (int)middleTile.y - r;
                }

                //Set up the position of the new tile
                Vector2 tilePos = Vector2.zero;
                tilePos.x = offset.x * diffC;
                tilePos.y = offset.y * diffR;

                //Spawn the tile
                GameObject newTileObj = GameObject.Instantiate(tilePrefab);
                newTileObj.transform.parent = tileParent;
                newTileObj.transform.position = tilePos;
                newTileObj.name = "Tile (" + r + "x" + c + ")";

                //Setup the tile's ID
                Tile newTile = newTileObj.GetComponent<Tile>();
                newTile.SetTileID(r, c);

                //Add it to the list
                allTiles.Add(newTile);
            }
        }
    }

    //Remove all tiles on the map
    public void RemoveTiles()
    {
        while(allTiles.Count > 0)
        {
            GameObject.Destroy(allTiles[0].gameObject);
            allTiles.RemoveAt(0);
        }

        numFlowers = 0;
    }

    //Clears all plants from all tiles
    public void ClearTiles()
    {
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].GetTileType() != Tile.TileType.NONE)
            {
                PruneTile(allTiles[i]);
            }
        }
        numFlowers = 0;
    }

/**
 * Randomized Placements 
 */
    //Spawn weeds randomly on the map
    public void SpawnWeeds(int numWeeds, int rows, int cols)
    {        
        for (int i = 0; i < numWeeds; i++)
        {
            bool weedPlaced = false;
            int tries = 0; //just here to stop infinite loops

            do //Find a tile to put the weed and put it in there
            {
                //Get a random row & col to place a weed
                int randRow = Random.Range(0, rows);
                int randCol = Random.Range(0, cols);

                int indexOfTile = GetIndexOfTile(randRow, randCol);

                //If this tile doesn't already have something thing
                if (allTiles[indexOfTile].GetTileType() == Tile.TileType.NONE)
                {
                    //Make it have a weed
                    SpawnWeed(allTiles[indexOfTile]);

                    weedPlaced = true;
                }
                tries++;
            } while (!weedPlaced && tries < 25);
        }
    }
    
    //Spawns flowers randomly on the map
    public void SpawnFlowers(int numFlowers, int rows, int cols)
    {
        for (int i = 0; i < numFlowers; i++)
        {
            bool flowerPlaced = false;
            int tries = 0; //just here to stop infinite loops

            do //Find a tile to put the flower and put it in there
            {
                //Get a random row & col to plant a flower
                int randRow = Random.Range(0, rows);
                int randCol = Random.Range(0, cols);

                int indexOfTile = GetIndexOfTile(randRow, randCol);

                //If this tile doesn't already have something planted
                if (allTiles[indexOfTile].GetTileType() == Tile.TileType.NONE)
                {
                    //Plant the flower
                    PlantFlower(allTiles[indexOfTile]);

                    flowerPlaced = true;
                }
                tries++;
            } while (!flowerPlaced && tries < 25);
        }
    }
    
/**
 *  Gameplay 
 */
    //Spawn a weed at a given tile
    public void SpawnWeed(Tile tile)
    {
        allTiles[GetIndexOfTile(tile.GetRow(), tile.GetColumn())].SetTileType(Tile.TileType.WEED);
        allTiles[GetIndexOfTile(tile.GetRow(), tile.GetColumn())].SetSprite(weedSprite);
    }

    //Spawn a flower at a given tile
    public void PlantFlower(Tile tile)
    {
        allTiles[GetIndexOfTile(tile.GetRow(), tile.GetColumn())].SetTileType(Tile.TileType.FLOWER);
        allTiles[GetIndexOfTile(tile.GetRow(), tile.GetColumn())].SetSprite(flowerSprite);

        numFlowers++;
    }

    //Remove whatever was at a given tile
    public void PruneTile(Tile tile)
    {
        if (tile.GetTileType() == Tile.TileType.FLOWER)
        {
            numFlowers--;
        }

        allTiles[GetIndexOfTile(tile.GetRow(), tile.GetColumn())].SetTileType(Tile.TileType.NONE);
        allTiles[GetIndexOfTile(tile.GetRow(), tile.GetColumn())].SetSprite(null);
    }

    //Play game of life
    public void NextGeneration(int numRows, int numCols)
    {
        tilesToChange = new Dictionary<Tile, Tile.TileType>(); //We don't want the changes to occur until after all the calculations are made, so keep track of what tiles are changing and what they're changing to

        for (int i = 0; i < allTiles.Count; i++)
        {
            Tile tile = allTiles[i];
            Dictionary<Tile.TileType, int> neighbors = GetNumberOfNeighbors(tile, numRows, numCols);
            int numNeighbors = neighbors[Tile.TileType.FLOWER] + neighbors[Tile.TileType.WEED];

            //If an empty tile has 3 neighbors
            if (tile.GetTileType() == Tile.TileType.NONE)
            {
                if (numNeighbors == 3)
                {
                    //If it has more flower neighbors than weeds, plant a flower
                    if (neighbors[Tile.TileType.FLOWER] > neighbors[Tile.TileType.WEED])
                    {
                        tilesToChange.Add(tile, Tile.TileType.FLOWER);
                    }
                    else //Otherwise, spawn a weed
                    {
                        tilesToChange.Add(tile, Tile.TileType.WEED);
                    }
                }
            }
            else
            {
                //If a weed or flower has less than 2 neighbors, it dies
                if (numNeighbors < 2 || numNeighbors > 3)
                {
                    tilesToChange.Add(tile, Tile.TileType.NONE);
                }
            }
        }

        UpdateTiles();
    }

    //Actually updates the tiles with what they should be
    private void UpdateTiles()
    {
        foreach(KeyValuePair<Tile, Tile.TileType> kvp in tilesToChange)
        {
            switch (kvp.Value)
            {
                case Tile.TileType.FLOWER: PlantFlower(kvp.Key);
                    break;
                case Tile.TileType.WEED: SpawnWeed(kvp.Key);
                    break;
                case Tile.TileType.NONE: PruneTile(kvp.Key);
                    break;
            }
        }
    }

/**
 * Other
 */

    public void SetTileParent(Transform parent)
    {
        tileParent = parent;
    }
}

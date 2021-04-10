using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Managers
    [SerializeField] private TileManager tileManager;
    [SerializeField] private AudioController audioController;

    //Pausing
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private bool gamePaused;

    //Level
    [SerializeField] private Level curLevel;

    //Turns
    [SerializeField] private Turn curTurn = new Turn();
    [SerializeField] private int turnNum;
    [SerializeField] private List<Tile> tilesAtTurnStart; //So that we can reset the turn to undo our changes
    [SerializeField] private bool turnStarted = false;
    [SerializeField] private bool restartingTurn = false;
    [SerializeField] private bool levelEnded = false;

    //Sandbox
    [SerializeField] private bool sandboxMode = false;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        PlayerInput();
    }

    private void LateUpdate()
    {
        audioController.PlayMusic();
    }

    //Starts a new turn
    public void StartTurn()
    {
        if (!levelEnded && !turnStarted)
        {
            restartingTurn = false;

            turnStarted = true;
            SaveTiles();
            curTurn.StartTurn();
        }
    }

    //Restarts the current turn
    public void ResetTurn()
    {
        if(turnStarted)
        {
            tileManager.SetTiles(tilesAtTurnStart);

            restartingTurn = true;
            turnStarted = false;
        }
    }

    //Ends the current turn
    public void EndTurn()
    {
        if (turnStarted)
        {
            tileManager.NextGeneration(curLevel.numRows, curLevel.numCols);

            turnStarted = false;
            turnNum++;
            curTurn.EndTurn();

            if (GetNumFlowers() > curLevel.numFlowersNeeded)
            {
                WinLevel();
            }
            else if (turnNum > curLevel.maxTurns)
            {
                LoseLevel();
            }
        }
    }

    //Ends the level by winning
    private void WinLevel()
    {
        levelEnded = true;
        audioController.PlaySound(AudioController.SFXType.WIN);
        curLevel.ShowWinningUI(true);
    }

    //Ends the level by losing
    private void LoseLevel()
    {
        levelEnded = true;
        audioController.PlaySound(AudioController.SFXType.LOSE);
        curLevel.ShowLosingUI(true);
    }

    //Saves the tiles in case you reset the turn
    public void SaveTiles()
    {
        List<Tile> allTiles = tileManager.GetTiles();

        tilesAtTurnStart = new List<Tile>();
        for (int i = 0; i < allTiles.Count; i++)
        {
            tilesAtTurnStart.Add(new Tile());
            tilesAtTurnStart[i].SetTileID(allTiles[i].GetTileID());
            tilesAtTurnStart[i].SetTileType(allTiles[i].GetTileType());
        }
    }

    //Resets the tiles to what they were at the start of the turn
    public void ResetTiles()
    {
        tileManager.SetTiles(tilesAtTurnStart);
    }

    //Spawn all the tiles on the map
    public void SpawnTiles()
    {
        tileManager.SpawnTiles(curLevel.numRows, curLevel.numCols);
    }

    //Remove all the tiles on the map
    public void RemoveTiles()
    {
        tileManager.RemoveTiles();
    }

    //Removes all flowers and weeds from all the tiles on the map
    public void ClearTiles()
    {
        tileManager.ClearTiles();
    }

    //Spawns Flowers and Weeds randomly on the map
    public void SpawnPlants()
    {
        SpawnFlowers();
        SpawnWeeds();
    }

    //Spawn flowers randomly across the tiles
    private void SpawnFlowers()
    {
        tileManager.SpawnFlowers(curLevel.numFlowersSpawn, curLevel.numRows, curLevel.numCols);
    }

    //Spawn weeds randomly across the tiles 
    private void SpawnWeeds()
    {
        tileManager.SpawnWeeds(curLevel.numWeedsSpawn, curLevel.numRows, curLevel.numCols);
    }

    //Throw in whatever requires input
    private void PlayerInput()
    {
        OnLeftClick();
        OnRightClick();
    }

    //If we left click on an empty tile, we want to spawn a flower there
    private void OnLeftClick()
    {
        if (turnStarted || sandboxMode)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector2 cursorPos = Input.mousePosition;
                Vector2 cursorScreenPos = Camera.main.ScreenToWorldPoint(cursorPos);

                RaycastHit2D hit = Physics2D.Raycast(cursorScreenPos, Vector2.zero);

                if (hit.collider != null && hit.transform.tag.Equals("Tile") && (curTurn.actionsLeft > 0 || sandboxMode))
                {
                    Tile tile = hit.transform.GetComponent<Tile>();
                    if (tile.GetTileType() == Tile.TileType.NONE)
                    {
                        curTurn.UseAction();
                        audioController.PlaySound(AudioController.SFXType.PLANT);
                        tileManager.PlantFlower(tile);
                    }
                }
            }
        }
    }

    //If we right click on an empty tile, we want to remove whatever was there
    private void OnRightClick()
    {
        if (turnStarted || sandboxMode)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Vector2 cursorPos = Input.mousePosition;
                Vector2 cursorScreenPos = Camera.main.ScreenToWorldPoint(cursorPos);

                RaycastHit2D hit = Physics2D.Raycast(cursorScreenPos, Vector2.zero);

                if (hit.collider != null && hit.transform.tag.Equals("Tile") && (curTurn.actionsLeft > 0 || sandboxMode))
                {
                    Tile tile = hit.transform.GetComponent<Tile>();
                    if (tile.GetTileType() != Tile.TileType.NONE)
                    {
                        curTurn.UseAction();
                        audioController.PlaySound(AudioController.SFXType.PRUNE);
                        tileManager.PruneTile(tile);
                    }
                }
            }
        }
    }

    //Loads a new level based on whatever is in the current scene
    public void LoadLevel()
    {
        curLevel = GameObject.FindGameObjectWithTag("LevelController").GetComponent<Level>();
        tileManager.SetTileParent(curLevel.tileParent);

        if (curLevel.GetType() == typeof(SandboxMode))
        {
            sandboxMode = true;
        }

        curTurn.actionsLeft = 0;
        curLevel.StartLevel();
    }

    //Perform the next generation of plants
    public void NextGeneration()
    {
        tileManager.NextGeneration(curLevel.numRows, curLevel.numCols);
    }

    //Get the number of flowers on the map
    public int GetNumFlowers()
    {
        return tileManager.numFlowers;
    }

    //Get the current turn
    public int GetNumTurn()
    {
        return turnNum;
    }

    //Basically to use at the end of a level
    public void ResetTurnNum()
    {
        turnNum = 0;
        levelEnded = false;
    }

    //Get the number of actions left your have this turn
    public int GetActionsLeft()
    {
        return curTurn.actionsLeft;
    }
    
    //Go back to the main menu
    public void BackToMainMenu()
    {
        RemoveTiles();
        ResetTurnNum();
        SceneManager.LoadScene("MainMenu");
    }
}

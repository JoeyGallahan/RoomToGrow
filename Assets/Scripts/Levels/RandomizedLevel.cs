using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class RandomizedLevel : Level
{
    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD
    }

    [SerializeField] private Difficulty difficulty;
    [SerializeField] private TextMeshProUGUI flowersText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI actionsText;

    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button restartButton;

    public override void Init()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                numFlowersSpawn = 3;
                numWeedsSpawn = 0;
                numRows = 5;
                numCols = 5;
                numFlowersNeeded = 10;
                break;
            case Difficulty.NORMAL:
                numFlowersSpawn = 3;
                numWeedsSpawn = 3;
                numRows = 6;
                numCols = 6;
                numFlowersNeeded = 15;
                break;
            case Difficulty.HARD:
                numFlowersSpawn = 4;
                numWeedsSpawn = 8;
                numRows = 8;
                numCols = 8;
                numFlowersNeeded = 20;
                break;
            default: Debug.Log("How did you even choose this difficulty?");
                break;
        }
        base.Init();

        startButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public override void StartLevel()
    {
        gameController.SpawnTiles();
        gameController.SpawnPlants();
    }

    protected override void DoUpdates()
    {
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        flowersText.SetText(gameController.GetNumFlowers().ToString() + " / " + numFlowersNeeded.ToString());
        turnText.SetText(gameController.GetNumTurn().ToString() + " / " + maxTurns.ToString());
        actionsText.SetText(gameController.GetActionsLeft().ToString());
    }

    public void StartTurn()
    {
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        gameController.StartTurn();
    }

    public void EndTurn()
    {
        startButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        gameController.EndTurn();
    }

    public void RestartTurn()
    {
        startButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        gameController.ResetTurn();
    }

    public void LoadNewLevel()
    {
        gameController.ClearTiles();
        gameController.ResetTurnNum();

        ShowLosingUI(false);
        ShowWinningUI(false);
        StartLevel();
    }

    public void BackToMainMenu()
    {
        gameController.BackToMainMenu();
    }
}

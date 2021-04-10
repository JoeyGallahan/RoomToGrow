using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SandboxMode : Level
{
    [SerializeField] private TMP_InputField rowInput;
    [SerializeField] private TMP_InputField colInput;

    [SerializeField] private bool started = false;
    [SerializeField] private bool paused = false;

    [SerializeField] GameObject startButton, pauseButton, resetButton;
    [SerializeField] TextMeshProUGUI pauseButtonText, generationText;
    
    protected override void DoUpdates()
    {
        if (started && !paused)
        {
            GameOfLife();
        }
    }

    public override void Init()
    {
        base.Init();
        pauseButton.SetActive(false);
        resetButton.SetActive(false);
    }

    public override void StartLevel()
    {
        GetNumTiles();
        gameController.SpawnTiles();
    }

    public void GetNumTiles()
    {
        int rows = 0;
        int cols = 0;

        int.TryParse(rowInput.text, out rows);
        int.TryParse(colInput.text, out cols);

        if (rows > 0 && cols > 0)
        {
            numRows = rows;
            numCols = cols;
        }
    }

    public void StartPlaying()
    {
        gameController.SaveTiles();
        gameController.NextGeneration();

        started = true;

        startButton.SetActive(false);
        pauseButton.SetActive(true);
        resetButton.SetActive(true);
    }

    public void PausePlay()
    {
        if (paused)
        {
            paused = false;
            pauseButtonText.SetText("Pause");
        }
        else
        {
            paused = true;
            pauseButtonText.SetText("Play");
        }
    }

    public void ResetPlay()
    {
        paused = false;
        started = false;

        generation = 1;
        generationText.SetText(generation.ToString());
        pauseButtonText.SetText("Pause");

        timeElapsed = 0.0f;

        gameController.ResetTiles();

        startButton.SetActive(true);
        pauseButton.SetActive(false);
        resetButton.SetActive(false);
    }

    private void GameOfLife()
    {
        if (timeElapsed < timeToChange)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0.0f;
            generation++;
            generationText.SetText(generation.ToString());
            gameController.NextGeneration();
        }
    }

    public void SpawnTiles()
    {
        GetNumTiles();
        gameController.SpawnTiles();
    }

    public void ClearTiles()
    {
        gameController.ClearTiles();
    }

    public void RemoveTiles()
    {
        GetNumTiles();
        gameController.RemoveTiles();
    }

}

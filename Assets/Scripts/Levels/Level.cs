using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public abstract class Level:MonoBehaviour
{
    //Presets for spawning
    public int numRows { get; protected set; } = 8;
    public int numCols { get; protected set; } = 8;
    public int numFlowersSpawn { get; protected set; } = 4;
    public int numWeedsSpawn { get; protected set; } = 0;
    public int maxTurns { get; protected set; } = 5;
    public int numFlowersNeeded { get; protected set; } = 10;

    [SerializeField] protected float timeToChange = 1.0f;
    [SerializeField] protected float timeElapsed = 0.0f;
    [SerializeField] protected int generation = 1;

    [SerializeField] public Transform tileParent;
    [SerializeField] protected GameController gameController;
    [SerializeField] private GameObject winningUI;
    [SerializeField] private GameObject losingUI;

    private void Start()
    {
        Init();
    }

    private void LateUpdate()
    {
        DoUpdates();
    }

    protected abstract void DoUpdates();

    public virtual void Init()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ShowWinningUI(false);
        ShowLosingUI(false);

        gameController.LoadLevel();
    }

    protected void NextGeneration()
    {
        gameController.NextGeneration();
    }

    public abstract void StartLevel();

    public void ShowWinningUI(bool maybe)
    {
        if (this.GetType() != typeof(SandboxMode))
        {
            winningUI.SetActive(maybe);
        }
    }

    public void ShowLosingUI(bool maybe)
    {
        if (this.GetType() != typeof(SandboxMode))
        {
            losingUI.SetActive(maybe);
        }
    }
}

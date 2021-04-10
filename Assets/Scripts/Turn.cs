using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turn
{
    [SerializeField] private int maxActions = 3;
    [SerializeField] public int actionsLeft = 3;

    public void StartTurn()
    {
        actionsLeft = maxActions;
    }

    public void UseAction()
    {
        actionsLeft--;
    }

    public void EndTurn()
    {
        actionsLeft = 0;
    }
}

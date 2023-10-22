using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public event EventHandler OnTurnChanged;
    private int turnNumber = 1;
    bool isPlayerTurn = true;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystem! " + transform + " . " + Instance);
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber += 1;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}

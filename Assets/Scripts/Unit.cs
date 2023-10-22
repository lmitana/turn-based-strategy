using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Grid;
using Turnbased.Actions;

public class Unit : MonoBehaviour
{
    const int ACTION_POINTS_MAX = 2;
    
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    [SerializeField] bool isEnemy;
    GridPosition gridPosition;
    HealthSystem healthSystem;
    BaseAction[] baseActionsArray;
    int actionPoints = ACTION_POINTS_MAX;
    
    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();        
        baseActionsArray = GetComponents<BaseAction>();
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition) {              
            GridPosition oldGridPosition = gridPosition;            
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionsArray)
            if (baseAction is T)
        {
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionsArray()
    {
        return baseActionsArray;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool CanSpendActionPoints(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }
    
    public bool TrySpendActionPoints(BaseAction baseAction)
    {
        if (!CanSpendActionPoints(baseAction))
        {
            return false;
        }

        SpendActionPoints(baseAction.GetActionPointsCost());
        return true;
    }

    void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    void TurnSystem_OnTurnChanged(object Sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    void HealthSystem_OnDead(object Sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
}

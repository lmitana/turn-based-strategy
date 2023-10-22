using System;
using System.Collections;
using System.Collections.Generic;
using Turnbased.Grid;
using UnityEngine;

namespace Turnbased.Actions
{
    public class MeleeAction : BaseAction
    {
        public event EventHandler OnMeleeActionStarted;
        public event EventHandler OnMeleeActionCompleted;
        [SerializeField] int maxAttackDistance = 2;        
        enum State
        {
            SwingBeforeHit,
            SwingAfterHit,
        }
        State state;
        float stateTimer;
        Unit targetUnit;

        void Update()
        {
            if (!isActive)
            {
                return;
            }

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.SwingBeforeHit:
                    Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                    float rotationSpeed = 20f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                    break;
                case State.SwingAfterHit:
                    OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
                    break;

            }
            if (stateTimer <= 0f)
            {
                NextState();
            }            
        }

        void NextState()
        {
            switch (state)
            {
                case State.SwingBeforeHit:
                    state = State.SwingAfterHit;
                    float afterHitStateTime = 0.1f;
                    stateTimer = afterHitStateTime;
                    targetUnit.Damage(100); // Magic number
                    break;
                case State.SwingAfterHit:
                    ActionComplete();
                    break;                
            }
        }

        public override string GetActionName()
        {
            return "Melee";
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction {
                gridPosition = gridPosition,
                actionValue = 200,
            };
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPosition();

            for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
            {
                for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // Grid is empty, no units
                        continue;
                    }

                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                        // Both units in same team
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }
            return validGridPositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            state = State.SwingBeforeHit;
            float beforeHitStateTime = 0.5f;
            stateTimer = beforeHitStateTime;

            OnMeleeActionStarted?.Invoke(this, EventArgs.Empty);
            ActionStart(onActionComplete);
        }

        public int GetMaxAttackDistance()
        {
            return maxAttackDistance;
        }
    }
}
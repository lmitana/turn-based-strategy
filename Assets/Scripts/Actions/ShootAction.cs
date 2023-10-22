using System;
using System.Collections;
using System.Collections.Generic;
using Turnbased.Grid;
using UnityEngine;

namespace Turnbased.Actions
{
    public class ShootAction : BaseAction
    {
        public event EventHandler<OnShootEventArgs> OnShoot;
        public static event EventHandler<OnShootEventArgs> OnAnyShoot;        
        public class OnShootEventArgs : EventArgs
        {
            public Unit targetUnit;
            public Unit shootingUnit;
        }
        int maxShootDistance = 4;
        [SerializeField] LayerMask obstaclesLayer;
        
        enum State
        {
            Aiming,
            Shooting,
            Cooloff,
        }

        State state;        
        float stateTimer;
        Unit targetUnit;
        bool canShoot;

        void Update()
        {
            if (!isActive)
            {
                return;
            }

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.Aiming:
                    Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                    float rotationSpeed = 20f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                    break;
                case State.Shooting:
                    if (canShoot)
                    {
                        Shoot();
                        canShoot = false;
                    }
                    break;
                case State.Cooloff:
                    break;
            }

            if (stateTimer <= 0f)
            {
                NextState();
            }
        }

        public override string GetActionName()
        {
            return "Shoot";
        }

        public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -maxShootDistance; x <= maxShootDistance; x++)
            {
                for (int z = -maxShootDistance; z <= maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxShootDistance)
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
                        // Both units in same 'team'
                        continue;
                    }

                    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDirection = targetUnit.GetWorldPosition() - unitWorldPosition.normalized;
                    float unitShoulderHeight = 1.7f;
                    if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDirection,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        obstaclesLayer))
                        {
                            // Blocked by an obstacle
                            continue;
                        }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = unit.GetGridPosition();
            return GetValidActionGridPositionList(unitGridPosition);
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            state = State.Aiming;
            float aimingStateTime = 1f;
            stateTimer = aimingStateTime;
            canShoot = true;

            ActionStart(onActionComplete);
        }

        public Unit GetTargetUnit()
        {
            return targetUnit;
        }

        void NextState()
        {
            switch (state)
            {
                case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                    break;
                case State.Shooting:
                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                    break;
                case State.Cooloff:
                    ActionComplete();
                    break;
            }            
        }

        void Shoot()
        {
            OnAnyShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = targetUnit,
                shootingUnit = unit
            });

            // TODO: Avoid usage of magic number
            targetUnit.Damage(40);

            OnShoot?.Invoke(this, new OnShootEventArgs {
                targetUnit = targetUnit,
                shootingUnit = unit
            });

            // TODO: Avoid usage of magic number
            targetUnit.Damage(40);
        }

        public int GetMaxShootDistance()
        {
            return maxShootDistance;
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);            

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
            };
        }
    }
}
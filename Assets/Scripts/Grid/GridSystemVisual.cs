using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Actions;

namespace Turnbased.Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        public static GridSystemVisual Instance { get; private set; }

        [Serializable]
        public struct GridVisualTypeMaterial
        {
            public GridVisualType gridVisualType;
            public Material material;
        }

        public enum GridVisualType
        {
            White,
            Blue,
            Red,
            RedSoft,
            Yellow,
        }

        [SerializeField] Transform gridVisualSinglePrefab;
        [SerializeField] List <GridVisualTypeMaterial> gridVisualTypeMaterials;
        GridSystemVisualSingle[,] gridVisualSingleArray;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than one UnitActionSystem! " + transform + " . " + Instance);
                Destroy(gameObject);
            }
            Instance = this;
        }

        void Start()
        {
            gridVisualSingleArray = new GridSystemVisualSingle[
                LevelGrid.Instance.GetWidth(),
                LevelGrid.Instance.GetHeight()
            ];

            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform gridSystemVisualSingleTransform =
                        Instantiate(gridVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                    gridVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                }
            }

            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
            UpdateGridVisual();
        }

        public void HideAllGridPosition()
        {
            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    gridVisualSingleArray[x, z].Hide();;
                }
            }
        }

        void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > range)
                    {
                        continue;
                    }

                    gridPositions.Add(testGridPosition);
                }
                ShowGridPositionList(gridPositions, gridVisualType);
            }
        }

        void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    gridPositions.Add(testGridPosition);
                }
                ShowGridPositionList(gridPositions, gridVisualType);
            }
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
        {
            foreach (GridPosition gridPosition in gridPositionList)
            {
                gridVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
            }
        }

        void UpdateGridVisual()
        {
            HideAllGridPosition();

            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

            GridVisualType gridVisualType;
            
            switch (selectedAction)
            {
                default:                    
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case SpinAction spinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
                case GrenadeAction grenadeAction:
                    gridVisualType = GridVisualType.Yellow;
                    break;
                case ShootAction shootAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                    break;
                case MeleeAction meleeAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), meleeAction.GetMaxAttackDistance(), GridVisualType.RedSoft);
                    break;
                case InteractAction interactAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
            }

            ShowGridPositionList(
                selectedAction.GetValidActionGridPositionList(), gridVisualType
            );
        }

        void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateGridVisual();
        }

        void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
        {
            UpdateGridVisual();
        }

        Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterials)
            {
                if (gridVisualTypeMaterial.gridVisualType == gridVisualType)                    
                {
                    return gridVisualTypeMaterial.material;
                }
            }
            Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
            return null;
        }
    }    
}
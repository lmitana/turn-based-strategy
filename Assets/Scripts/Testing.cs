using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Grid;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit;

    void Update()
    {      
        if (Input.GetKeyDown(KeyCode.T))
        {
            ScreenShake.Instance.Shake(5f);
            /*
            GridPosition mouseGridPostion = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPostion);

            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i +1]),
                    Color.white,
                    10f
                );
            }
            */
        }        
    }
}

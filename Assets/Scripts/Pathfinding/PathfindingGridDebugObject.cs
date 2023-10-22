using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Turnbased.Grid;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] TextMeshPro gCostText;
    [SerializeField] TextMeshPro hCostText;
    [SerializeField] TextMeshPro fCostText;
    [SerializeField] SpriteRenderer isWalkableSpriteRenderer;
    PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;        
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}

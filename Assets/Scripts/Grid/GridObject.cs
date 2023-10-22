using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turnbased.Grid
{
    public class GridObject
    {
        GridSystem<GridObject> gridSystem;
        GridPosition gridPosition;
        List<Unit> units;
        private IInteractable interactable;

        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            units = new List<Unit>();
        }

        public override string ToString()
        {
            string unitString = "";
            foreach (Unit unit in units)
            {
                unitString += unit + "\n";
            }
            return gridPosition.ToString() + "\n" + unitString;
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
        }

        public List<Unit> GetUnits()
        {
            return units;
        }

        public bool HasAnyUnit()
        {
            return units.Count > 0;
        }

        public Unit GetUnit()
        {
            if (HasAnyUnit())
            {
                return units[0];
            }
            return null;
        }

        public IInteractable GetInteractable()
        {
            return interactable;
        }

        public void SetInteractable(IInteractable interactable)
        {
            this.interactable = interactable;
        }
    }

}
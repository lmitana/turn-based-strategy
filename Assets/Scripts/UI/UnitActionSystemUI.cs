using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Turnbased.Actions;

namespace Turnbased.UI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] Transform actionButtonPrefab;
        [SerializeField] Transform actionButtonContainer;
        [SerializeField] TextMeshProUGUI actionPointsText;

        private List<ActionButtonUI> actionButtonUIList;

        void Awake()
        {
            actionButtonUIList = new List<ActionButtonUI>();
        }

        void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

            UpdateActionPoints();
            CreateUnitActionButtons();
            UpdateSelectedVisual();
        }

        void CreateUnitActionButtons()
        {
            foreach (Transform buttonTransform in actionButtonContainer)
            {
                Destroy(buttonTransform.gameObject);
            }

            actionButtonUIList.Clear();

            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

            foreach (BaseAction baseAction in selectedUnit.GetBaseActionsArray())
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);

                actionButtonUIList.Add(actionButtonUI);
            }
        }

        void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
        }

        void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        void UpdateSelectedVisual()
        {
            foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }

        void UpdateActionPoints()
        {
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }

        void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }
    }
}
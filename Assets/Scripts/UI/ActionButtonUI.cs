using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Turnbased.Actions;

namespace Turnbased.UI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textMeshPro;
        [SerializeField] Button button;
        [SerializeField] GameObject selectedGameObject;

        BaseAction baseAction;

        public void SetBaseAction(BaseAction baseAction)
        {
            this.baseAction = baseAction;
            textMeshPro.text = baseAction.GetActionName();

            button.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
        }

        public void UpdateSelectedVisual()
        {
            BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
            selectedGameObject.SetActive(selectedBaseAction == baseAction);
        }
    }
}
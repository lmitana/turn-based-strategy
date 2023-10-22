using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Turnbased.UI
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] Button endTurnButton;
        [SerializeField] TextMeshProUGUI turnNumberText;
        [SerializeField] GameObject enemyTurnUIGameObject;

        void Start()
        {
            endTurnButton.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });

            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

            UpdateTurnText();
            UpdateEnemyTurnUI();
            UpdateEndturnButtonVisibility();
        }

        void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateTurnText();
            UpdateEnemyTurnUI();
            UpdateEndturnButtonVisibility();
        }

        void UpdateTurnText()
        {
            turnNumberText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
        }

        void UpdateEnemyTurnUI()
        {
            enemyTurnUIGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
        }

        void UpdateEndturnButtonVisibility()
        {
            endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn()); 
        }
    }
}
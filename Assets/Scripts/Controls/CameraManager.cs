using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Actions;

namespace Turnbased.Controls
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] GameObject actionCameraGameObject;

        void Start()
        {
            BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
            BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

            HideActionCamera();
        }

        void ShowActionCamera()
        {
            actionCameraGameObject.SetActive(true);
        }

        void HideActionCamera()
        {
            actionCameraGameObject.SetActive(false);
        }

        void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    Unit shooterUnit = shootAction.GetUnit();
                    Unit targetUnit = shootAction.GetTargetUnit();

                    Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                    Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                    float shoulderOffsetAmount = 0.5f;
                    Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

                    Vector3 actionCameraPosition =
                        shooterUnit.GetWorldPosition() +
                        cameraCharacterHeight +
                        shoulderOffset +
                        (shootDirection * -1);

                    actionCameraGameObject.transform.position = actionCameraPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                    ShowActionCamera();

                    break;
            }
        }

        void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:                   
                    HideActionCamera();
                    break;
            }
        }
    }
}
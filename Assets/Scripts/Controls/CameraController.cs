using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turnbased.Controls
{
    public class CameraController : MonoBehaviour
    {
        void Update()
        {
            Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();

            float moveSpeed = 10f;
            Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
    }
}
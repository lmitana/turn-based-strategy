using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one InputManager! " + transform + " . " + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    public Vector2 GetMouseScreenPosition()
    {
        return Input.mousePosition;
    }

    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveDirection = new Vector3(0, 0);
        
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }

        /*
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }
        */

        return inputMoveDirection;
    }
}

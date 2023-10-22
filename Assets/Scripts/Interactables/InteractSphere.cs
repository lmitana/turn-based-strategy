using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Grid;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] Material greenMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] MeshRenderer meshRenderer;

    GridPosition gridPosition;
    bool isGreen;
    Action onInteractComplete;
    float timer;
    bool isActive;

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        
        SetColorGreen();
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractComplete();
        }    
    }

    void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }
    
    void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.5f;

        if (isGreen)
        {
            SetColorRed();
        } else
        {
            SetColorGreen();
        }
    }
}
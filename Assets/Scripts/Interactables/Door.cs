using System;
using System.Collections;
using System.Collections.Generic;
using Turnbased.Grid;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool isOpen;
    GridPosition gridPosition;
    Animator animator;

    Action onInteractComplete;
    float timer;
    bool isActive;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        if (isOpen)
        {
            Open();
        } else
        {
            Close();
        }
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

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.5f;

        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    void Open()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    void Close()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
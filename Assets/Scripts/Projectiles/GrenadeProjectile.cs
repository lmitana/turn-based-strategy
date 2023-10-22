using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Grid;

public class GrenadeProjectile : MonoBehaviour    
{
    public static event EventHandler OnAnyGrenadeExploded;
    [SerializeField] Transform grenateExplodeVfx;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] AnimationCurve arcYCurve;
    [SerializeField] float maxHeight = 3f;
    
    Vector3 targetPosition;
    Vector3 positionXZ;
    float totalDistance;
    Action onGrenadeBehaviourComplete;

    void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        
        positionXZ += moveDirection * moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;        
        float positionY = arcYCurve.Evaluate(distanceNormalized) * totalDistance / maxHeight;

        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = 0.2f; // Magic number
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f; // Magic number
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(40); // Magic number
                }

                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;
            Instantiate(grenateExplodeVfx, targetPosition + Vector3.up * 1, Quaternion.identity);
            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}

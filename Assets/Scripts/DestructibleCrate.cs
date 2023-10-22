using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turnbased.Grid;

public class DestructibleCrate : MonoBehaviour
{
   public static event EventHandler OnAnyDestroyed;
   [SerializeField] Transform createDestroyedPrefab;
   GridPosition gridPosition;

   void Start()
   {
      gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
   }

   public GridPosition GetGridPosition()
   {
      return gridPosition;
   }

   public void Damage()
   {
      Transform crateDestroyed = Instantiate(createDestroyedPrefab, transform.position, transform.rotation);
      ApplyExplosion(crateDestroyed, 150f, transform.position, 10f);

      Destroy(gameObject);
      OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
   }

    void ApplyExplosion(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosion(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}

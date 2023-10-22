using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turnbased.Projectiles
{
    public class BulletProjectile : MonoBehaviour
        {
            [SerializeField] TrailRenderer trailRenderer;
            [SerializeField] Transform bulletHitVfxPrefab;
            Vector3 targetPosition;

            public void Setup(Vector3 targetPosition)
            {
                this.targetPosition = targetPosition;
            }

            void Update()
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;

                float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

                float moveSpeed = 200f;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

                float distanceAtterMoving = Vector3.Distance(transform.position, targetPosition);

                if (distanceBeforeMoving < distanceAtterMoving)
                {
                    transform.position = targetPosition;
                    trailRenderer.transform.parent = null;
                    Destroy(gameObject);

                    Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);
                }
            }
        }
}
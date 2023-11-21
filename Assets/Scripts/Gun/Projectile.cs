using System;
using UnityEngine;

namespace ProjectZee
{
    public class Projectile : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]
        [SerializeField] private Color trailColor;
        [SerializeField] private LayerMask collisionMask;
        [Space]
        [SerializeField][Range(1f, 100f)] private float projectileSpeed = 10f;
        [SerializeField][Range(1f, 100f)] private float projectileMaxRange = 10f;

        private float distanceTraveled = 0f;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Start()
        {
           /* Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);

            if(initialCollisions.Length > 0 )
            {
                OnHitObject(initialCollisions[0], transform.position);
            }*/

            GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
        }

        private void Update()
        {
            ProjectileMovement();
        }

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        private void ProjectileMovement()
        {
            float moveDistance = projectileSpeed * Time.deltaTime;
            CheckCollisions(moveDistance);
            transform.Translate(projectileSpeed * Time.deltaTime * Vector3.forward);

            // Update the distance traveled
            distanceTraveled += moveDistance;

            // Check if the projectile has reached its maximum range
            if (distanceTraveled >= projectileMaxRange) KillProjectile();
        }

        private void CheckCollisions(float moveDistance)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) OnHitObject(hit);
        }

        private void OnHitObject(RaycastHit hit)
        {
            print(hit.transform.name);
            KillProjectile();
        }

        public void SetProjectileRange(float newRange)
        {
            projectileMaxRange = newRange;
        }

        public void SetProjectileSpeed(float newSpeed)
        {
            projectileSpeed = newSpeed;
        }

        private void KillProjectile()
        {
            Destroy(gameObject);
        }

        #endregion

        // ----------------------------------------------------------------------
    }
}

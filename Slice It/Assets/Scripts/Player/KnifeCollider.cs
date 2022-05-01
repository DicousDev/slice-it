using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SliceIt.ScriptableObjects.Utils.Events;

using SliceIt.Slices;

namespace SliceIt.Knife
{
    public sealed class KnifeCollider : MonoBehaviour
    {
        [SerializeField] private GameEvent onAddPoint = default;
        [SerializeField] private GameEvent onGameOver = default;
        [SerializeField] private Rigidbody rb;
        private Collider thisCollider;
        private Transform thisTransform;

        [SerializeField] private float delayToStartDetectorColliderInSeconds = 0.11f;
        [SerializeField] private float distanceOfRaycastToCheckTipsOfKnife = 0.09f;
        [SerializeField] private LayerMask maskToTipsOfKnife;

        [SerializeField] private float explosionForce = 0.4f;
        [SerializeField] private float explosionRadius = 0.11f;
        [SerializeField] private float upwards = 0.06f;
        [SerializeField] private LayerMask explosionLayer;

        private bool isSleepingKnife = false;

        private void Awake()
        {
            thisTransform = this.transform;
            thisCollider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            checkIfTheKnifeTipIsCollidingWithRaycast();
        }

        public void OnEnabledColliderInSeconds()
        {
            StartCoroutine(EnableColliderInSecondsCoroutine());
        }

        private IEnumerator EnableColliderInSecondsCoroutine()
        {
            yield return new WaitForSeconds(delayToStartDetectorColliderInSeconds);
            thisCollider.enabled = true;
            isSleepingKnife = false;
        }

        private void checkIfTheKnifeTipIsCollidingWithRaycast()
        {
            RaycastHit hit;
            Ray rayDirection = new Ray(thisTransform.position, thisTransform.right);
            bool isColliding = Physics.Raycast(rayDirection, out hit, distanceOfRaycastToCheckTipsOfKnife, maskToTipsOfKnife);
            Debug.DrawRay(thisTransform.position, thisTransform.right * distanceOfRaycastToCheckTipsOfKnife, Color.red);
            /*            Collider[] colliding = Physics.OverlapSphere(thisTransform.position, distanceOfRaycastToCheckTipsOfKnife, maskToTipsOfKnife);
                        bool isColliding = colliding.Length > 0;*/

            if (isColliding)
            {
                SleepKnife();
            }

            if (hit.collider == null)
                return;

            KnifeColliderTypes(hit.collider.gameObject);
        }

        private void Explosion()
        {
            Collider[] slices = Physics.OverlapSphere(thisTransform.position, explosionRadius, explosionLayer);
            foreach(Collider slice in slices)
            {
                Rigidbody rb = slice.gameObject.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.AddExplosionForce(explosionForce, thisTransform.position, explosionRadius, upwards, ForceMode.Impulse);
                }
            }
        }

        private void SleepKnife()
        {
            if(!isSleepingKnife)
            {
                rb.isKinematic = true;
                thisCollider.enabled = false;
                isSleepingKnife = true;                
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            KnifeColliderTypes(collider.gameObject);
        }

        private void KnifeColliderTypes(GameObject target)
        {
            Slice slice = target.GetComponent<Slice>();
            if (slice != null)
            {
                Slice(slice);
            }

            GameOverCollider gameOverCollider = target.GetComponent<GameOverCollider>();
            if (gameOverCollider != null)
            {
                GameOver();
            }
        }

        private void Slice(Slice slice)
        {
            slice.Execute();
            AddPointsAfterSlicing();
            Explosion();
        }

        private void GameOver()
        {
            onGameOver.Raise();
        }

        private void AddPointsAfterSlicing()
        {
            onAddPoint.Raise();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, distanceOfRaycastToCheckTipsOfKnife);
        }
    }
}

using System.Collections;
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

        [SerializeField] private float delayToStartDetectorColliderInSeconds = 1.0f;
        [SerializeField] private float distanceOfRaycastToCheckTipsOfKnife = 0.045f;
        [SerializeField] private LayerMask maskToTipsOfKnife;

        private bool isSleepingKnife = true;

        private void Awake()
        {
            thisTransform = this.transform;
            thisCollider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            checkIfTheKnifeTipIsCollidingWithRaycast();
        }

        public void EnabledColliderInSeconds()
        {
            StartCoroutine(EnableColliderInSecondsCoroutine());
        }

        private IEnumerator EnableColliderInSecondsCoroutine()
        {
            yield return new WaitForSeconds(delayToStartDetectorColliderInSeconds);
            thisCollider.enabled = true;
            isSleepingKnife = true;
        }

        private void checkIfTheKnifeTipIsCollidingWithRaycast()
        {
            RaycastHit hit;
            Ray rayDirection = new Ray(thisTransform.position, thisTransform.right);
            bool isColliding = Physics.Raycast(rayDirection, out hit, distanceOfRaycastToCheckTipsOfKnife, maskToTipsOfKnife);
            Debug.DrawRay(thisTransform.position, thisTransform.right * distanceOfRaycastToCheckTipsOfKnife, Color.red);

            if(isColliding)
            {
                SleepKnife();
            }

            if(hit.collider == null)
                return;

            KnifeColliderTypes(hit.collider.gameObject);
        }

        private void SleepKnife()
        {
            rb.isKinematic = true;
            thisCollider.enabled = false;

            if (isSleepingKnife)
            {
                isSleepingKnife = false;
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
        }

        private void GameOver()
        {
            onGameOver.Raise();
        }

        private void AddPointsAfterSlicing()
        {
            onAddPoint.Raise();
        }
    }
}

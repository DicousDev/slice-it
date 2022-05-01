using System.Collections;
using UnityEngine;
using SliceIt.ScriptableObjects.Utils.Events;

using SliceIt.Slices;

namespace SliceIt.Knife
{
    public sealed class KnifeCollider : MonoBehaviour
    {
        [SerializeField] private GameEvent onAddPoint;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider collider;
        private Transform thisTransform;

        [SerializeField] private float delayToStartDetectorColliderInSeconds = 1.0f;

        [SerializeField] private float distanceOfRaycastToCheckTipsOfKnife = 0.045f;
        [SerializeField] private LayerMask maskToTipsOfKnife;

        private bool isSleepingKnife = true;

        private void Awake()
        {
            thisTransform = this.transform;
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
            collider.enabled = true;
            isSleepingKnife = true;
        }

        private void SleepKnife()
        {
            rb.isKinematic = true;
            collider.enabled = false;

            if (isSleepingKnife)
            {
                isSleepingKnife = false;
            }
        }

        private void Slice(Slice slice)
        {
            slice.Execute();
            AddPointsAfterSlicing();
        }

        private void AddPointsAfterSlicing()
        {
            onAddPoint.Raise();
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

            Slice slice = hit.collider.gameObject.GetComponent<Slice>();
            if(slice == null)
                return;

            Slice(slice);
        }

        private void OnTriggerEnter(Collider collider)
        {
            Ground isGround = collider.GetComponent<Ground>();
            if(isGround != null)
            {
                SleepKnife();
            }

            Slice canSlice = collider.GetComponent<Slice>();
            if(canSlice != null)
            {
                Slice(canSlice);
            }
        }
    }
}

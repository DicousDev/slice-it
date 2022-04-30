using System.Collections;
using UnityEngine;

namespace Desafio.CameraGame
{ 
    public sealed class FollowKnife : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField] private Transform knifeTarget;
        [Range(0.01f, 1)]
        [SerializeField] private float followSmooth = 0.5f;
        private Transform thisTransform;

        private void Awake()
        {
            mainCamera = Camera.main;
            thisTransform = this.transform;
        }

        private void LateUpdate()
        {
            LookToTarget();
            FollowTarget();
        }

        private void LookToTarget()
        {
            thisTransform.LookAt(knifeTarget, Vector3.up);
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = thisTransform.position;
            targetPosition.x = knifeTarget.position.x;
            thisTransform.position = Vector3.Lerp(thisTransform.position, targetPosition, followSmooth);
        }
    }
}
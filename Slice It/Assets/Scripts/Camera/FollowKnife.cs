using UnityEngine;
using SliceIt.ScriptableObjects.Utils.Events;

namespace SliceIt.CameraGame
{ 
    public sealed class FollowKnife : MonoBehaviour
    {
        [SerializeField] private GameEvent onStartGame = default;
        private Camera mainCamera;
        [SerializeField] private Transform knifeTarget;
        [Range(0.01f, 1)]
        [SerializeField] private float followSmooth = 0.5f;
        private Transform thisTransform;
        private bool canFollow = false;

        private void OnEnable()
        {
            onStartGame.onGameEvent += EnabledFollow;
        }

        private void OnDisable()
        {
            onStartGame.onGameEvent -= EnabledFollow;
        }

        private void Awake()
        {
            mainCamera = Camera.main;
            thisTransform = this.transform;
        }

        private void LateUpdate()
        {
            LookToTarget();

            if (!canFollow) return;

            FollowTarget();
        }

        private void EnabledFollow()
        {
            canFollow = true;
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
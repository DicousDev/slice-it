using UnityEngine;
using UnityEngine.Events;

namespace SliceIt.Knife
{
    public sealed class KnifeController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float forceToUpShoot = 200;
        [SerializeField] private float torqueShoot = 80;
        private bool readyToShoot = false;
        private bool readyAgainShoot = false;

        [Range(30, 85)]
        [SerializeField] private float angleToShoot = 85;
        private Vector3 directionToShoot;

        [SerializeField] private float delayToAgainShootInSeconds = 0.5f;
        private float delayToAgainShootInSecondsCurrent;

        [SerializeField] private UnityEvent onKnifeShooted;
        private KnifeInputActions knifeInputActions;

        private void OnEnable()
        {
            knifeInputActions.Enable();
        }

        private void OnDisable()
        {
            knifeInputActions.Disable();
        }

        private void Awake()
        {
            knifeInputActions = new KnifeInputActions();
        }

        private void Start()
        {
            CalculateDirectionToShoot();
        }

        private void Update()
        {
            if(CanStartShoot())
            {
                StartShoot();
            }

            CheckDelayTimeToAgainShoot();
        }

        private void FixedUpdate()
        {
            if(readyToShoot)
            {
                Shoot();
            }
        }

        private bool CanStartShoot()
        {
            return readyAgainShoot && knifeInputActions.Shoot.Shoot.triggered;
        }

        private void StartShoot()
        {
            readyToShoot = true;
            readyAgainShoot = false;
            ResetTimeToAgainShoot();
            if (rb.isKinematic)
            {
                onKnifeShooted?.Invoke();
            }
        }

        private void ResetTimeToAgainShoot()
        {
            delayToAgainShootInSecondsCurrent = delayToAgainShootInSeconds;
        }

        private void Shoot()
        {
            rb.isKinematic = false;
            readyToShoot = false;
            rb.AddForce(directionToShoot * forceToUpShoot);
            rb.AddTorque(Vector3.forward * torqueShoot);
        }

        private void CheckDelayTimeToAgainShoot()
        {
            delayToAgainShootInSecondsCurrent -= Time.deltaTime;
            if (delayToAgainShootInSecondsCurrent <= 0)
            {
                readyAgainShoot = true;
            }
        }

        private void CalculateDirectionToShoot()
        {
            float x = Mathf.Cos(angleToShoot * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleToShoot * Mathf.Deg2Rad);
            directionToShoot = new Vector3(x, y, 0);
        }
    }
}

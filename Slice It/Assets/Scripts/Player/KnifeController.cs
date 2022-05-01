using UnityEngine;
using UnityEngine.Events;

namespace SliceIt.Knife
{
    public sealed class KnifeController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float forceToUpShoot = 200;
        [SerializeField] private float torqueShoot = 80;
        private bool canShoot = false;
        private bool canAgainShoot = false;

        [Range(30, 85)]
        [SerializeField] private float degressToShoot = 85;
        private Vector3 directionToShoot;

        [SerializeField] private float delayToAgainShoot = 0.5f;
        private float delayToAgainShootCurrent;

        [SerializeField] private UnityEvent onKnifeAttacked;

        private void Start()
        {
            CalculateDirectionToForce();
        }

        private void Update()
        {
            if(canAgainShoot && Input.GetMouseButtonDown(0))
            {
                canShoot = true;
                canAgainShoot = false;
                delayToAgainShootCurrent = delayToAgainShoot;
                if (rb.isKinematic)
                {
                    onKnifeAttacked?.Invoke();
                }
            }

            CheckDelayTimeToAgainShoot();
        }

        private void FixedUpdate()
        {
            if(canShoot)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            rb.isKinematic = false;
            canShoot = false;
            rb.AddForce(directionToShoot * forceToUpShoot);
            rb.AddTorque(Vector3.forward * torqueShoot);
        }

        private void CheckDelayTimeToAgainShoot()
        {
            delayToAgainShootCurrent -= Time.deltaTime;
            if (delayToAgainShootCurrent <= 0)
            {
                canAgainShoot = true;
            }
        }

        private void CalculateDirectionToForce()
        {
            float x = Mathf.Cos(degressToShoot * Mathf.Deg2Rad);
            float y = Mathf.Sin(degressToShoot * Mathf.Deg2Rad);
            directionToShoot = new Vector3(x, y, 0);
        }
    }
}

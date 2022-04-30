using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCollider : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider collider;
    [SerializeField] private float delayToStartDetectorColliderInSeconds = 1.0f;

    public void EnabledColliderInSeconds()
    {
        StartCoroutine(EnableColliderCoroutine());
    }

    private IEnumerator EnableColliderCoroutine()
    {
        yield return new WaitForSeconds(delayToStartDetectorColliderInSeconds);
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Ground isGround = collider.GetComponent<Ground>();
        if(isGround != null)
        {
            this.collider.enabled = false;
            rb.isKinematic = true;
        }
    }
}

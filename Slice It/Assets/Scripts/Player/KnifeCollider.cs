using System.Collections;
using UnityEngine;

using Desafio.Slices;

public sealed class KnifeCollider : MonoBehaviour
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

    private void SleepKnife()
    {
        collider.enabled = false;
        rb.isKinematic = true;
    }

    private void Slice(Slice slice)
    {
        slice.Execute();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float forceToUp;
    [SerializeField] private bool canJump = false;
    [SerializeField] private float torque;
    [Range(30, 85)]
    [SerializeField] private float degressToForce = 85;
    [SerializeField] private Vector3 directionToForce;
    [SerializeField] private UnityEvent onKnifeAttacked;

    private void Start()
    {
        CalculateDirectionToForce();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            canJump = true;
            onKnifeAttacked?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if(canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.isKinematic = false;
        canJump = false;
        rb.AddForce(directionToForce * forceToUp);
        rb.AddTorque(Vector3.forward * torque);
    }

    private void CalculateDirectionToForce()
    {
        float x = Mathf.Cos(degressToForce * Mathf.Deg2Rad);
        float y = Mathf.Sin(degressToForce * Mathf.Deg2Rad);
        directionToForce = new Vector3(x, y, 0);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameOverCollider isGameOver = collision.gameObject.GetComponent<GameOverCollider>();
        if (isGameOver != null)
        {
            GameOver();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float destroyDelay = 10f; // Time to wait before destroying the ball

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("ball created");
        // Start the coroutine to destroy the ball
        StartCoroutine(DestroyBall());
    }
    
    void Update() {
        Debug.Log(transform.position);
    }

    private IEnumerator DestroyBall()
    {
        // Wait for the specified delay before destroying the ball
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject); // Destroy the ball GameObject
    }
}
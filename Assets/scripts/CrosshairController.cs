using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject boundingBox;       // The object that defines the bounds.
    public GameObject ballPrefab;        // The ball prefab to be instantiated.
    public Transform spawnPoint;
    public float moveInterval = 0.5f;      // Time interval between crosshair movements.
    public float jitterDuration = 0.5f;    // Duration of jittering before locking on.
    public float ballSpeed = 100f;        // Speed of the ball.
    public CameraShake cameraShake;      // Reference to the CameraShake script
    public float shakeDuration = 0.5f;   // Duration of the shake
    public float shakeMagnitude = 0.1f;  // Magnitude of the shake
    public SoundManager soundManager;    // Reference to the SoundManager script
    public GameManager GameManager;      // Reference to GameManager

    private Vector3 finalPosition;       // Final random position for the crosshair.
    private Vector3 boundingBoxMin;      // Min bounds of the bounding box.
    private Vector3 boundingBoxMax;      // Max bounds of the bounding box.
    
    private int ballsSpawned = 0;        // Number of balls spawned
    private int crosshairEntries = 0;    // Number of times player entered crosshair

    void Start()
    {
        // Get the bounds of the bounding box object
        Renderer renderer = boundingBox.GetComponent<Renderer>();
        boundingBoxMin = renderer.bounds.min;
        boundingBoxMax = renderer.bounds.max;

        // Start the process of moving the crosshair
        StartCoroutine(MoveCrosshair());
    }

    IEnumerator MoveCrosshair()
    {
        while (true)
        {
            soundManager.PlayTickingSound();
            // Get a random position within the bounding box
            finalPosition = GetRandomPosition();

            // Create an array to hold the random target positions
            Vector3[] randomPositions = new Vector3[9];

            // Generate 9 random positions around the final target
            for (int i = 0; i < 9; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.5f, 0.5f), 
                    Random.Range(-0.5f, 0.5f), 
                    0f
                );
                randomPositions[i] = finalPosition + randomOffset;
            }

            // Jittering: Lerp between all random positions
            float elapsed = 0f;
            float durationPerMove = jitterDuration / randomPositions.Length; // Time for each move
            Vector3 initialPosition = transform.position;

            for (int i = 0; i < randomPositions.Length; i++)
            {
                Vector3 targetPosition = randomPositions[i];
                elapsed = 0f; // Reset elapsed time
                
                while (elapsed < durationPerMove)
                {
                    elapsed += Time.deltaTime;
                    transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / durationPerMove);
                    yield return null;
                }
                
                // Update the initial position to the current target position for the next lerp
                initialPosition = targetPosition;
            }

            // Snap the crosshair to the final position after jittering
            transform.position = finalPosition;

            // Spawn the ball and launch it toward the final position
            soundManager.StopTickingSound();
            yield return new WaitForSeconds(0.25f);
            LaunchBall();

            // Wait for the next movement interval
            yield return new WaitForSeconds(moveInterval);
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(boundingBoxMin.x, boundingBoxMax.x);
        float randomY = Random.Range(boundingBoxMin.y, boundingBoxMax.y);
        return new Vector3(randomX, randomY, transform.position.z);
    }

    void LaunchBall()
    {
        Debug.Log("launch ball");
        soundManager.PlayWhooshSound();
        Vector3 spawnPoint = new Vector3(finalPosition.x - 1f, finalPosition.y - 1f, -1);

        // Instantiate the ball at the spawnPoint position
        GameObject ball = Instantiate(ballPrefab, spawnPoint, Quaternion.identity);

        // Calculate the direction vector from the spawn position towards the crosshair (finalPosition)
        Vector3 direction = (finalPosition - spawnPoint).normalized;

        // Add velocity to the ball towards the crosshair using Rigidbody
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = direction * ballSpeed * 5; // Apply velocity

        // Track the number of balls spawned and check for life deduction
        ballsSpawned++;
        CheckBallVsCrosshair();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        // soundManager.PlayPumpSound();

        if (other.CompareTag("Player"))
        {
            crosshairEntries++;
            CheckBallVsCrosshair();
        }

        // Ensure that 'other' has a Rigidbody component
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Stop the ball's movement
            rb.velocity = Vector3.zero; // Set velocity to zero
            cameraShake.Shake(shakeDuration, shakeMagnitude);
        }
    }

    private void CheckBallVsCrosshair()
    {
        if (ballsSpawned > crosshairEntries)
        {
            GameManager.DeductLife();
        }
    }
}

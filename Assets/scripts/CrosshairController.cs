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
    public CameraShake cameraShake; // Reference to the CameraShake script
    public float shakeDuration = 0.5f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    public SoundManager soundManager; // Reference to the SoundManager script

    private Vector3 finalPosition;       // Final random position for the crosshair.
    private Vector3 boundingBoxMin;      // Min bounds of the bounding box.
    private Vector3 boundingBoxMax;      // Max bounds of the bounding box.

    private  HealthBar hb; 

    private bool playerIsInside = false;

    void Start()
    {
        hb = GameObject.FindGameObjectWithTag("healthmanager").GetComponent<HealthBar>(); 
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
    Vector3 spawnPoint = new Vector3(finalPosition.x - 1f, finalPosition.y - 1f, 1);

    // Instantiate the ball at the spawnPoint position
    GameObject ball = Instantiate(ballPrefab, spawnPoint, Quaternion.identity);
    
    // Set the target position in the ball's controller
    // Assign the target position
    //Debug.Log(finalPosition);
    
    // Calculate the direction vector from the spawn position towards the crosshair (finalPosition)
    Vector3 direction = (finalPosition - spawnPoint).normalized;

    // Add velocity to the ball towards the crosshair using Rigidbody
    Rigidbody rb = ball.GetComponent<Rigidbody>();

    // Apply velocity in 3D space
    rb.velocity = direction * ballSpeed*5; // Velocity for X, Y, and Z axes
    }

//    void OnTriggerEnter(Collider other)
//     {
//         // soundManager.PlayPumpSound();
//         // Check if the ball has collided with the crosshair trigger
//         // Ensure that 'other' has a Rigidbody component
//         Rigidbody rb = other.GetComponent<Rigidbody>();
//         if (other.gameObject.tag == "ball")
//         {
//             Debug.Log("missed ball");
//             // Stop the ball's movement
//             rb.velocity = Vector3.zero; // Set the velocity to zero
//             cameraShake.Shake(shakeDuration, shakeMagnitude);
//             hb.LoseLife();
//             // rb.isKinematic = true; // Optionally make it kinematic
//         }
//     }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInside = true; // Player is touching the crosshair
        }
        if (other.gameObject.CompareTag("ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            cameraShake.Shake(shakeDuration, shakeMagnitude);

            // If the player is NOT inside, count this as a miss
            if (!playerIsInside)
            {
                soundManager.PlayPumpSound();
                Debug.Log("Missed ball");
                // Stop the ball's movement
                hb.LoseLife(); // Reduce life only if it’s a miss
            }
            else
            {
                soundManager.PlayClinkSound();
                Debug.Log("Player caught the ball, not a miss!");
            }
        } 
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInside = false; // Player has left the crosshair
        }
    }

}

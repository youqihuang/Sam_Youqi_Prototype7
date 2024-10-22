using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private Vector3 originalPosition; // To store the original position of the camera

    private void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        originalPosition = mainCamera.transform.localPosition; // Store the original position
    }

    // Public method to trigger screen shake
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Generate a random offset for shaking
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply the offset to the camera's position
            mainCamera.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Reset the camera position back to original
        mainCamera.transform.localPosition = originalPosition;
    }
}
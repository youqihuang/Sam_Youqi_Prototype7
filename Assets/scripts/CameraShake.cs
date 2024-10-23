using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
        private Vector3 originalPosition;  // To store the original position of the camera
    private Coroutine shakeCoroutine;  // To store the reference to the shake coroutine

    void Start()
    {
        originalPosition = transform.localPosition;  // Store the initial camera position
    }

    // Method to start shaking the camera
    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);  // Stop any current shaking
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));  // Start the shake
    }

    // Coroutine for the shaking effect
    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Randomly shake the camera within the magnitude range
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            yield return null;  // Wait for the next frame
        }

        // After shaking, return the camera to its original position
        transform.localPosition = originalPosition;
    }

    // Method to stop the shake immediately
    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);  // Stop the shake coroutine
            shakeCoroutine = null;
        }

        // Reset the camera to its original position
        transform.localPosition = originalPosition;
    }
}
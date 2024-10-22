using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip whooshSound;  // Sound to play when the ball is instantiated
    public AudioClip pumpSound;     // Sound to play when the ball hits the crosshair
    public AudioClip tickingSound;
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Add an AudioSource component to the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayWhooshSound()
    {
        audioSource.PlayOneShot(whooshSound);
    }

    public void PlayPumpSound()
    {
        audioSource.PlayOneShot(pumpSound);
    }

    public void PlayTickingSound()
    {
        audioSource.PlayOneShot(tickingSound);
    }

    public void StopTickingSound(float fadeDuration = 1f)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.Stop(); // Stop the sound after fading out
        audioSource.volume = startVolume; // Reset the volume for future use
    }

}

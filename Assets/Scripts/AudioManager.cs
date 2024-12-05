using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private AudioSource mainAudioSource;
    [SerializeField]
    private AudioSource winAudioSource;
    [SerializeField]
    private AudioSource loseAudioSource;
    private float maxYPosition = 500f;

    void Start()
    {
        mainAudioSource.Play();
    }

    void Update()
    {
        AdjustAudioPitch();
    }

    private void AdjustAudioPitch()
    {
        float playerY = player.transform.position.y;

        playerY = Mathf.Clamp(playerY, 0, maxYPosition);

        mainAudioSource.pitch = Mathf.Lerp(1.0f, 1.4f, playerY / maxYPosition);
    }

    public void PlayWinSound()
    {
        winAudioSource.PlayOneShot(winAudioSource.clip);
    }

    public void PlayLoseSound()
    {
        loseAudioSource.PlayOneShot(loseAudioSource.clip);
    }
}

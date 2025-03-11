using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip flipSound, matchSound, mismatchSound, gameOverSound;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFlipSound() => audioSource.PlayOneShot(flipSound);
    public void PlayMatchSound() => audioSource.PlayOneShot(matchSound);
    public void PlayMismatchSound() => audioSource.PlayOneShot(mismatchSound);
    public void PlayGameOverSound() => audioSource.PlayOneShot(gameOverSound);
}
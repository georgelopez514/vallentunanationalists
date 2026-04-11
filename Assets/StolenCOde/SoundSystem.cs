using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public bool activateAudrio;
    public bool activateWalkingSound;
    public AudioSource walkingaudioSource;
    public AudioSource audioSource;
    public AudioClip[] walkingSounds;
    public AudioClip[] talkingSounds;
    public AudioClip[] AudioTriggers;

    private void Awake()
    {
        // Check if audioSystem is assigned
        if (walkingaudioSource == null && audioSource == null)
        {
            Debug.LogError("AudioSource not assigned in the Inspector."); // Log an error if AudioSource is not assigned
        }

        // Optionally assign the audioClip to the audioSystem if not done in the Inspector
        if (walkingaudioSource != null && audioSource != null && walkingSounds != null)
        {
            walkingaudioSource.clip = walkingSounds[1]; // Assign the audio clip to the audio source
        }
    }

    private void Update()
    {
        walkingAudio();
    }

    public void walkingAudio()
    {
        if (activateWalkingSound == true) {
            walkingaudioSource.clip = null; // prevents overlapping
            walkingaudioSource.clip = walkingSounds[Random.Range(0, walkingSounds.Length)]; // plays audio
            walkingaudioSource.Play();
            activateWalkingSound = false;
        }
    }
}

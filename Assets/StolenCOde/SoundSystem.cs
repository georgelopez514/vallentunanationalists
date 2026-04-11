using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    Interaction interaction;

    public int talkingIndex;
    public bool activateAudrio;
    public bool activateWalkingSound;
    public AudioSource walkingaudioSource;
    public AudioSource audioSource;
    public AudioClip[] walkingSounds;
    public AudioClip[] talkingSounds;
    public AudioClip[] AudioTriggers;

    private void Awake()
    {
        talkingIndex = 0;

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
        if (interaction.EventCaller() == "npc")
        {
            Debug.Log("[SoundSystem] talking...");
            npctalking();
        }
    }

    public void npctalking()
    {
        if (activateAudrio)
        {
            audioSource.clip = null; // prevents overlapping
            audioSource.clip = talkingSounds[talkingIndex];
            walkingaudioSource.Play();
        }

        if (talkingIndex > talkingSounds.Length)
        {
            talkingIndex = 0;
        }
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

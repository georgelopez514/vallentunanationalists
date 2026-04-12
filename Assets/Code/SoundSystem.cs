using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public Interaction interaction;

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

        if (walkingaudioSource == null && audioSource == null)
        {
            Debug.LogError("AudioSource not assigned in the Inspector.");
        }

        if (walkingaudioSource != null && audioSource != null && walkingSounds != null)
        {
            walkingaudioSource.clip = walkingSounds[1];
        }
    }

    private void Update()
    {
        walkingAudio();
        Debug.Log("[SoundSystem] interaction.EventCaller()");

        if (interaction.EventCaller() == "npc" && !audioSource.isPlaying)
        {
            Debug.Log("[SoundSystem] talking...");
            activateAudrio = true;
            npctalking();
        }
    }

    public void npctalking()
    {
        if (activateAudrio)
        {
            audioSource.clip = talkingSounds[talkingIndex];
            audioSource.Play(); 
            activateAudrio = false;

            talkingIndex++; 

            if (talkingIndex >= talkingSounds.Length) 
            {
                talkingIndex = 0;
            }
        }
    }

    public void walkingAudio()
    {
        if (activateWalkingSound == true)
        {
            walkingaudioSource.clip = null;
            walkingaudioSource.clip = walkingSounds[Random.Range(0, walkingSounds.Length)];
            walkingaudioSource.Play();
            activateWalkingSound = false;
        }
    }
}
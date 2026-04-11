using UnityEngine;
using System.Collections;

public class PlayAudioDelayed : MonoBehaviour
{
    public AudioSource audioSource;
    public float delayTime = 3f;

    void Start()
    {
        StartCoroutine(PlaySoundAfterDelay());
    }

    IEnumerator PlaySoundAfterDelay()
    {

        yield return new WaitForSeconds(delayTime);


        audioSource.Play();
    }
}
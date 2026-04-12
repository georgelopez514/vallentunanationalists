using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    public bool killthemusic;
    public string musicObjectName = "djmusic";

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (killthemusic)
        {
            GameObject music = GameObject.Find(musicObjectName);
            if (music != null)
            {
                Destroy(music);
            }
        }
        else {
            DontDestroyOnLoad(GameObject.Find(musicObjectName));
        }

        Debug.Log($"[LevelTransitionTrigger] Loading: {levelToLoad}");
        SceneManager.LoadScene(levelToLoad);
    }
}
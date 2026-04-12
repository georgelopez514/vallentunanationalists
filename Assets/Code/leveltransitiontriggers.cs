using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string levelToLoad;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[LevelTransitionTrigger] Loading: {levelToLoad}");
        SceneManager.LoadScene(levelToLoad);
    }
}
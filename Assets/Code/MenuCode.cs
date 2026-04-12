using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCode : MonoBehaviour
{
    public string momba;

    public void StartGame()
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene(momba); // Replace with your scene name
    }

    // Method for quitting the game
    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}

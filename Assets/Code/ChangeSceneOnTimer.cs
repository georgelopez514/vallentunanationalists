using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    public string musicObjectName = "djmusic";

    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime < 0)
        {
            GameObject music = GameObject.Find(musicObjectName);
            Destroy(music);
            SceneManager.LoadScene(sceneName);
        }



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ChangeEnding : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public bool isPlayerStarted = false;

    void Update()
    {
        if (isPlayerStarted == false && VideoPlayer.isPlaying == true)
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }
        if (isPlayerStarted == true && VideoPlayer.isPlaying == false)
        {
            // Wehen the player stopped playing, hide it
          //  VideoPlayer.gameObject.SetActive(false);
            SceneManager.LoadScene("Start");
        }
    }
}

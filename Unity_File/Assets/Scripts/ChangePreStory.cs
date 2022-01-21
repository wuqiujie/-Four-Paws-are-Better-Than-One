using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ChangePreStory : MonoBehaviour
{
    public GameObject Canvas_Video;
    public GameObject Video;

    public VideoPlayer VideoPlayer;
    public bool isPlayerStarted = false;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("1");
        if (collision.gameObject.name== "Company")
        {
          
            Canvas_Video.SetActive(true);
            Video.SetActive(true);
        }
    }

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
            SceneManager.LoadScene("Selection");
        }

    }

  

}

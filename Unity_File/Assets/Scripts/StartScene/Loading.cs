using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    public GameObject IMG_Loading;
    public Slider slider;
    public TMP_Text progressText;
    public float playerValue = 0f;
    public string sceneName;

    bool p1Cooldown = false;
    bool startTakingInput = false;

    public void Update()
    {
        if (startTakingInput)
        { 
            if (!p1Cooldown && 
                (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                playerValue += 10f;
                p1Cooldown = true;
            }

            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G))
            {
                playerValue += 10f;
            }

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                p1Cooldown = false;
            }

            slider.value = playerValue / 100;
            progressText.text = playerValue + "%";

            if (playerValue == 100)
            {
                LoadScene(sceneName); 
            }
        }
    }

    public void PlayerPrepare(string sceneIndex)
    {
        if (startTakingInput) return;
        startTakingInput = true;
        GameObject.Find("PlayerSelection").GetComponent<PlayerSelectionController>().DisableControls();
        sceneName = sceneIndex;
        IMG_Loading.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

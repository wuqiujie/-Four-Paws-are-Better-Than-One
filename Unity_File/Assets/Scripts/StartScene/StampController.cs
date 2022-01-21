using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampController : MonoBehaviour
{
    public float waitTime;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Image>().color = new Color(255, 255, 255, 0);
        Invoke(nameof(ShowStamp), waitTime);
    }

    void ShowStamp()
    {
        GetComponent<Image>().color = new Color(255, 255, 255, 255);
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringController : MonoBehaviour
{
    public GameObject[] stars;

    public void SetScore(int score)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < score);
        }
    }
}

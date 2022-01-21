using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryBox : MonoBehaviour
{
    public Classification boxClassification;
    public GameObject paperStack;
    public Material correctMaterial;
    public Material incorrectMaterial;
    public GameObject correctParticles;
    public GameObject incorrectParticles;

    [Header("SFX")]
    public AudioClip correctClip;
    public AudioClip incorrectClip;

    Material baseMaterial;

    M1GameManager gameManager;
    AudioSource audioSource;

    MeshRenderer[] papers;

    void Start()
    {
        papers = paperStack.GetComponentsInChildren<MeshRenderer>();
        baseMaterial = papers[0].material;
        gameManager = GameObject.Find("GameManager").GetComponent<M1GameManager>();
        audioSource = GetComponent<AudioSource>();
    }


    public bool ReceivePaper(GameObject paper)
    {
        bool success;

        if (paper.GetComponent<Paper>().GetClassification() == boxClassification)
        {
            SetPapersMaterial(correctMaterial);
            correctParticles.SetActive(true);
            gameManager.AddToScore();

            audioSource.clip = correctClip;
            audioSource.Play();

            success = true;
        }
        else 
        {
            SetPapersMaterial(incorrectMaterial);
            incorrectParticles.SetActive(true);

            audioSource.clip = incorrectClip;
            audioSource.Play();

            success = false;
        }

        Invoke(nameof(SwitchBackToDefaultMat), 0.2f);
        Destroy(paper);

        return success;
    }

    void SetPapersMaterial(Material mat)
    {
        foreach (MeshRenderer paper in papers)
        {
            paper.material = mat;
        }
    }

    void SwitchBackToDefaultMat()
    {
        SetPapersMaterial(baseMaterial);
        correctParticles.SetActive(false);
        incorrectParticles.SetActive(false);
    }
}

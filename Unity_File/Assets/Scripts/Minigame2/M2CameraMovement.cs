using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2CameraMovement : MonoBehaviour
{
    public Animator sceneAnimator;
    public GameObject looksObjects;
    public GameObject gameplayObjects;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        looksObjects.SetActive(true);
        gameplayObjects.SetActive(false);
    }

    public void MoveCamera()
    {
        anim.SetTrigger("MoveCam");
        sceneAnimator.SetTrigger("MoveScene");
    }

    void SwapObjects()
    {
        looksObjects.SetActive(false);
        gameplayObjects.SetActive(true);
    }
}

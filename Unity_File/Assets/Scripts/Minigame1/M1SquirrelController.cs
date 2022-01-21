using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1SquirrelController : MonoBehaviour
{
    public GameObject paper;
    public GameObject animatedPaper;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void GrabPaper(Subject newSubject)
    {
        anim.SetTrigger("Grab");
        paper.GetComponent<Paper>().InitializePaper(newSubject);
        animatedPaper.GetComponent<Paper>().InitializePaper(newSubject);
    }

    public void PassPaper()
    {
        anim.SetTrigger("Pass");
    }

    public void BeginTapping()
    {
        anim.SetBool("IsTapping", true);
    }

    public void StopTapping()
    {
        anim.SetBool("IsTapping", false);
    }

    public void Success()
    {
        anim.SetTrigger("Win");
    }

    public void MakePaperVisible()
    {
        paper.SetActive(true);
    }

    public void MakePaperInvisible()
    {
        animatedPaper.GetComponent<Animator>().SetTrigger("Fly");
        paper.SetActive(false);
    }
}

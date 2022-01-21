using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1PikachuController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PassPaperLeft()
    {
        anim.SetTrigger("PassLeft");
    }

    public void PassPaperRight()
    {
        anim.SetTrigger("PassRight");
    }
    public void SetPassCorrectness(bool value)
    {
        anim.SetBool("CorrectPass", value);
    }

    public void Success()
    {
        anim.SetTrigger("Win");
    }
}

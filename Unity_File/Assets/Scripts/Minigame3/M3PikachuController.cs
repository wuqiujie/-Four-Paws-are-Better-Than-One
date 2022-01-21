using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3PikachuController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Pose(Action act)
    {
        switch (act)
        {
            case Action.F:
                anim.SetTrigger("PoseLeft");
                break;
            case Action.G:
                anim.SetTrigger("PoseRight");
                break;
        }
    }

    public void CorrectPoses()
    {
        anim.SetTrigger("Correct");
    }

    public void IncorrectPoses()
    {
        anim.SetTrigger("Incorrect");
    }

    public void Success()
    {
        anim.SetTrigger("Win");
    }
}

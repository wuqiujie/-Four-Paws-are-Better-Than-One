using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3SquirrelController : MonoBehaviour
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
            case Action.Up:
                anim.SetTrigger("PoseUp");
                break;
            case Action.Down:
                anim.SetTrigger("PoseDown");
                break;
            case Action.Left:
                anim.SetTrigger("PoseLeft");
                break;
            case Action.Right:
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
        anim.SetBool("Win", true);
    }
}

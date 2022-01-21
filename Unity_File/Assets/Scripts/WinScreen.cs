using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    bool p1keyDown = false;
    bool p2keyDown = false;

    void Update()
    {
        if (!p1keyDown)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                p1keyDown = true;
            }
        }

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            p1keyDown = false;
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G))
        {
            p2keyDown = true;
        }
        else 
        {
            p2keyDown = false;
        }

        if (p1keyDown && p2keyDown) ReturnToSelection();
    }

    void ReturnToSelection()
    {
        SceneManager.LoadScene("Selection");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDisplayerController : MonoBehaviour
{
    public Image upArrow;
    public Image leftArrow;
    public Image downArrow;
    public Image rightArrow;
    public Image fKey;
    public Image gKey;

    bool keyDown = false;

    void Update()
    {
        if (!keyDown)
        {
            if (Input.GetAxis("Vertical") > 0) HandleDirectionDown(upArrow);
            else if (Input.GetAxis("Horizontal") < 0) HandleDirectionDown(leftArrow);
            else if (Input.GetAxis("Vertical") < 0) HandleDirectionDown(downArrow);
            else if (Input.GetAxis("Horizontal") > 0) HandleDirectionDown(rightArrow);
        }
        else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) ClearAllDirections();

        if (Input.GetKeyDown(KeyCode.F))
        {
            fKey.color = Color.green;
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            gKey.color = Color.green;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            fKey.color = Color.white;
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            gKey.color = Color.white;
        }
    }

    void HandleDirectionDown(Image directionImage)
    {
        keyDown = true;
        directionImage.color = Color.green;
    }

    void ClearAllDirections()
    {
        keyDown = false;
        upArrow.color = Color.white;
        leftArrow.color = Color.white;
        downArrow.color = Color.white;
        rightArrow.color = Color.white;
    }
}

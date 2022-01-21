using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionController : MonoBehaviour
{
    public List<GameObject> buttons;

    int currentIndex = 0;
    bool ddrDown = false;
    bool disableControls = true;

    private void Start()
    {
        currentIndex = 0;
        if (buttons.Count > 0) buttons[currentIndex].GetComponent<Button>().Select();
        Invoke(nameof(AllowControls), 1f);
    }

    public void SetButtons(List<GameObject> buttons)
    {
        this.buttons = buttons;
        currentIndex = 0;
        buttons[currentIndex].GetComponent<Button>().Select();
    }

    void AllowControls()
    {
        disableControls = false;
    }

    private void Update()
    {
        if (disableControls) return;
        if (!ddrDown)
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") < 0)
            {
                ddrDown = true;
                currentIndex = Mathf.Min(currentIndex + 1, buttons.Count - 1);
                buttons[currentIndex].GetComponent<Button>().Select();
            }
            else if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0)
            {
                ddrDown = true;
                currentIndex = Mathf.Max(currentIndex - 1, 0);
                buttons[currentIndex].GetComponent<Button>().Select();
            }
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            ddrDown = false;
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G))
        {
            buttons[currentIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    public void DisableControls()
    {
        disableControls = true;
    }
}

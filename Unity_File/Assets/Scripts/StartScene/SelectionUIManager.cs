using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI
;
public class SelectionUIManager : MonoBehaviour
{
    public Button[] minigameButtons;
    public GameObject continueButton;

    public GameObject[] finishIcon;

    SelectionManager selectionManager;
    PlayerSelectionController playerSelection;

    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
       
        int numMinigames = selectionManager.GetNumMinigames();
        List<GameObject> buttons = new List<GameObject>();

        for (int i = 0; i < minigameButtons.Length; i++)
        {
            minigameButtons[i].interactable = i < numMinigames;
            if (i < numMinigames && !selectionManager.GetFinishedMinigames(i))
            {
                buttons.Add(minigameButtons[i].gameObject);
            }
        }

        for (int i=0;i< finishIcon.Length; i++)
        {
            finishIcon[i].SetActive(selectionManager.GetFinishedMinigames(i));
        }
        continueButton.SetActive(selectionManager.AllMinigamesCompleted());
        
        if (selectionManager.AllMinigamesCompleted())
        {
            buttons.Add(continueButton);
        }

        playerSelection = GameObject.Find("PlayerSelection").GetComponent<PlayerSelectionController>();
        playerSelection.SetButtons(buttons);
    }
}

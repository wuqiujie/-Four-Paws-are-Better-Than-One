using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M1GameManager : GameManager
{
    [Header("Minigame 1 Variables")]
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI accuracyText;

    public PrinterController printerController;
    public PapersController papersController;

    public Animator cameraAnimator;

    public override void StopShowingTutorial()
    {
        cameraAnimator.SetTrigger("MoveCam");
    }

    public override void StartOtherGameObjects()
    {
        printerController.StartSpawningPapers();
    }

    public override void UpdateScoreText()
    {
        totalText.text = $"Papers Sorted: {currentScore} / {maxScore}";
    }

    public override void DoOnWin()
    {
        papersController.WinGame();
        printerController.WinGame();
    }

    public override int GetGameIndex()
    {
        return 0;
    }
}

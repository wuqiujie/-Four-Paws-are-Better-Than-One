using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M3GameManager : GameManager
{
    [Header("Minigame 3 Variables")]
    public TextMeshProUGUI totalText;

    public ScreenController screenController;
    public Animator cameraAnimator;

    public override void StopShowingTutorial()
    {
        cameraAnimator.SetTrigger("MoveCam");
    }

    public override void StartOtherGameObjects()
    {
        screenController.StartCreatingGreetings();
    }

    public override void UpdateScoreText()
    {
        totalText.text = $"Greetings Memorized: {currentScore} / {maxScore}";
    }

    public override void DoOnWin()
    {
        screenController.WinGame();
    }

    public override int GetGameIndex()
    {
        return 2;
    }
}

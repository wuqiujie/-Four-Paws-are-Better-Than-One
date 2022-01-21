using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M2GameManager : GameManager
{
    [Header("Minigame 2 Variables")]
    public TextMeshProUGUI totalText;

    public M2PlayerController playerController;
    public M2CameraMovement cameraMovement;

    public override void StopShowingTutorial()
    {
        cameraMovement.MoveCamera();
    }

    public override void StartOtherGameObjects()
    {
        playerController.AllowMovement();
    }

    public override void UpdateScoreText()
    {
        totalText.text = $"Boxes Delivered: {currentScore} / {maxScore}";
    }

    public override void DoOnWin()
    {
        playerController.EndGame();
    }

    public override int GetGameIndex()
    {
        return 1;
    }
}

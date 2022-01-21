using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action { Up, Down, Left, Right, F, G }

public class ScreenController : MonoBehaviour
{
    public ActionsController screenActionsController;
    public ActionsController playerActionsController;

    public GameObject player1;
    public GameObject player2;

    public float inBetweenWaitTime = 0.5f;

    public GameObject tutorialPanel;

    public BossScreenController bossController;
    public M3SquirrelController squirrelController;
    public M3PikachuController pikachuController;

    int currNumSteps = 3;
    List<Action> currentActions;
    List<Action> currentGreeting;

    bool keyDown = false;
    bool acceptingInput = false;

    int chooseRange = 4;
    int balanceAmount = 2;

    M3GameManager gameManager;

    [Header("SFX")]
    public AudioClip correctClip;
    public AudioClip incorrectClip;
    public AudioClip countdownClip;
    AudioSource audioSource;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<M3GameManager>();
        currentActions = new List<Action>();
        currentGreeting = new List<Action>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartCreatingGreetings()
    {
        CreateGreeting();
        screenActionsController.AddListenerToSeqActions(FinishedDisplaying);
    }

    void FinishedDisplaying()
    {
        audioSource.clip = countdownClip;
        audioSource.Play();

        Invoke(nameof(StartListening), audioSource.clip.length - 0.5f);
    }

    void StartListening()
    {
        SetAnimatorsToFaceScreen(false);
        screenActionsController.HideActions();

        tutorialPanel.SetActive(true);
        acceptingInput = true;
    }

    void CreateGreeting()
    {
        currentActions.Clear();
        currentGreeting.Clear();

        for (int i = 0; i < currNumSteps; i++)
        {
            // Balance actions between two players
            int player = Random.Range(0, chooseRange);

            Action randAction;
            if (player < balanceAmount)
            {
                randAction = (Action)Random.Range(0, 4);
                balanceAmount--;
            }
            else
            {
                randAction = (Action)Random.Range(4, 6);
                balanceAmount++;
            } 

            currentActions.Add(randAction);
            currentGreeting.Add(randAction);
        }

        DisplayGreeting();
        currNumSteps++;
    }

    void DisplayGreeting()
    {
        tutorialPanel.SetActive(false);
        bossController.MakeBossOooh();

        SetAnimatorsToFaceScreen(true);
        screenActionsController.DisplayActionsSequentially(currentGreeting, inBetweenWaitTime);
        playerActionsController.HideActions();
        playerActionsController.SetPlaceholders(currentGreeting.Count);

        currentActions.Clear();
        currentActions.AddRange(currentGreeting);
    }

    void Update()
    {
        if (acceptingInput)
        { 
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") > 0) HandleAction(Action.Up);
                else if (Input.GetAxis("Horizontal") < 0) HandleAction(Action.Left);
                else if (Input.GetAxis("Vertical") < 0) HandleAction(Action.Down);
                else if (Input.GetAxis("Horizontal") > 0) HandleAction(Action.Right);
            }
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) keyDown = false;

            if (Input.GetKeyDown(KeyCode.F)) HandleAction(Action.F);
            else if (Input.GetKeyDown(KeyCode.G)) HandleAction(Action.G);
        }
    }

    void HandleAction(Action act)
    {
        if (currentActions.Count == 0) return;

        keyDown = true;

        if (currentActions[0] == act)
        {
            currentActions.RemoveAt(0);

            // Display correct action
            if (currentActions.Count != 0)
            { 
                if (act != Action.F && act != Action.G) squirrelController.Pose(act);
                else pikachuController.Pose(act);
            }

            playerActionsController.ShowCorrectAction(act);
        }
        else
        {
            acceptingInput = false;
            playerActionsController.ShowIncorrectAction(act);

            // Incorrect animations + sounds
            squirrelController.IncorrectPoses();
            pikachuController.IncorrectPoses();

            if (Random.Range(0, 1) == 0) bossController.MakeBossAngry();
            else bossController.MakeBossSad();

            audioSource.clip = incorrectClip;
            audioSource.Play();

            Invoke(nameof(DisplayGreeting), 2f);
        }

        if (currentActions.Count == 0)
        {
            acceptingInput = false;

            // Correct animations + sounds
            squirrelController.CorrectPoses();
            pikachuController.CorrectPoses();
            bossController.MakeBossHappy();

            audioSource.clip = correctClip;
            audioSource.Play();

            Invoke(nameof(GreetingCompleted), 3f);
        }
    }

    void GreetingCompleted()
    {
        gameManager.AddToScore();
        if (!gameManager.GameOver()) CreateGreeting();
    }

    void SetAnimatorsToFaceScreen(bool value)
    {
        Camera.main.gameObject.GetComponent<Animator>().SetBool("IsZoomed", value);
        player1.GetComponent<Animator>().SetBool("IsFacingScreen", value);
        player2.GetComponent<Animator>().SetBool("IsFacingScreen", value);
    }

    public void WinGame()
    {
        squirrelController.Success();
        pikachuController.Success();
    }
}

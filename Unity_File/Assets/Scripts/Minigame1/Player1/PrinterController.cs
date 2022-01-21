using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Down, Left, Right }

public class PrinterController : MonoBehaviour
{
    public GameObject paperObject;
    public float spawnTimeMin;
    public float spawnTimeMax;
    public GameObject smoke;

    public DirActionsController actionsController;
    public PapersController papersController;
    public M1SquirrelController squirrelController;

    List<Direction> currentActions = new List<Direction>(); // List of directions that need to be fulfilled to do current action
    List<Direction> passPaperActions = new List<Direction>();

    bool paperSpawned = false;
    bool printerBroken = false;
    bool keyDown = false;
    bool gameFinished = false;

    const int startingBreakThreshold = 20;
    int currentMinBreakThreshold;

    int minSteps = 3;
    int maxSteps = 6;
    int currMaxSteps;

    public int numPapersToPass;

    [Header("Printer SFX")]
    public AudioClip paperComeOutClip;
    public AudioClip printerBrokenClip;
    public AudioClip printerWorkingClip;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        passPaperActions.Add(Direction.Up);
        passPaperActions.Add(Direction.Right);
        currMaxSteps = minSteps + 1;
        currentMinBreakThreshold = startingBreakThreshold;
    }

    public void StartSpawningPapers()
    {
        StartCoroutine(SpawnPapers());
    }

    IEnumerator SpawnPapers()
    {
        while (!gameFinished)
        {
            yield return new WaitForSeconds(Random.Range(spawnTimeMin, spawnTimeMax));
            int rng = Random.Range(0, startingBreakThreshold + 1); 
            if (rng > currentMinBreakThreshold)
            {
                BreakPrinter();
            }
            else
            {
                SpawnPaper();
                currentMinBreakThreshold -= 1;
            }

            while (paperSpawned || printerBroken)
            { 
                yield return null;
            }
        }
    }

    void SpawnPaper()
    {
        paperObject.SetActive(true);
        paperSpawned = true;

        //currentActions.AddRange(passPaperActions);
        Direction randDir = (Direction)Random.Range(0, 4);
        currentActions.Add(randDir);
        currentActions.Add(Direction.Right);

        // Display actions
        actionsController.DisplayActions(currentActions);

        audioSource.clip = paperComeOutClip;
        audioSource.Play();
    }

    void BreakPrinter()
    {
        printerBroken = true;
        squirrelController.BeginTapping();

        int numActions = Random.Range(minSteps, currMaxSteps);
        for (int i = 0; i < numActions; i++)
        {
            Direction randDir = (Direction)Random.Range(0, 4);
            currentActions.Add(randDir);
        }

        if (Random.Range(0f, 1f) < 0.5f) currMaxSteps = Mathf.Min(currMaxSteps + 1, maxSteps);

        // Display actions
        actionsController.DisplayActions(currentActions);

        audioSource.clip = printerBrokenClip;
        audioSource.Play();

        smoke.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameFinished)
        {
            if (paperSpawned || printerBroken)
            {
                if (!keyDown)
                {
                    if (Input.GetAxis("Vertical") > 0) HandleAction(Direction.Up);
                    else if (Input.GetAxis("Horizontal") < 0) HandleAction(Direction.Left);
                    else if (Input.GetAxis("Vertical") < 0) HandleAction(Direction.Down);
                    else if (Input.GetAxis("Horizontal") > 0) HandleAction(Direction.Right);
                }
                if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) keyDown = false;
            }
        }
    }

    void HandleAction(Direction dir)
    {
        if (currentActions.Count == 0) return;
        
        keyDown = true;

        if (currentActions[0] == dir)
        {
            currentActions.RemoveAt(0);
            actionsController.CompletedAction();
        }
        else 
        {
            actionsController.IncorrectAction();
        }

        if (currentActions.Count == 1 && !printerBroken)
        {
            paperObject.SetActive(false);
            squirrelController.GrabPaper(papersController.SetNewSubject());
        }

        if (currentActions.Count == 0)
        {
            if (printerBroken)
            {
                // Fix printer
                Invoke(nameof(FixPrinter), 0.2f);
            }
            else
            {
                // Pass paper
                squirrelController.PassPaper();
                Invoke(nameof(PassPaper), 1.8f);
            }
        }
    }

    void PassPaper()
    {
        paperSpawned = false;

        // Pass paper to player 2
        for (int i = 0; i < numPapersToPass; i++)
        {
            papersController.SpawnPaper();
        }

        // Update actions display
        actionsController.HideActions();
    }

    void FixPrinter()
    {
        squirrelController.StopTapping();
        printerBroken = false;

        // Update actions display
        actionsController.HideActions();

        audioSource.clip = printerWorkingClip;
        audioSource.Play();

        smoke.SetActive(false);

        currentMinBreakThreshold = startingBreakThreshold;
    }

    public void WinGame()
    {
        gameFinished = true;
        squirrelController.Success();
    }
}

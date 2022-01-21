using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapersController : MonoBehaviour
{
    public GameObject paperPrefab;
    public CategoryBox animalBox;
    public CategoryBox humanBox;

    public M1PikachuController pikachuController;

    public GameObject currentPaperPosition;
    public GameObject otherPaperPosition;

    List<GameObject> papers = new List<GameObject>(); // Current paper is the one in index 0
    string subjectsFilePath = "Subjects";
    Subject[] animals;
    Subject[] humans;
    bool onCooldown = false;
    bool gameFinished = false;

    Subject newSubject = null;

    void Start()
    {
        TextAsset subjectsTextAsset = Resources.Load<TextAsset>(subjectsFilePath);
        Subjects subjects = JsonUtility.FromJson<Subjects>(subjectsTextAsset.text);
        Debug.Log(subjectsTextAsset.text);
        animals = subjects.animals;
        humans = subjects.humans;
    }

    public Subject SetNewSubject()
    {
        if (Random.Range(0, 2) == 0)
        {
            newSubject = humans[Random.Range(0, humans.Length)];
        }
        else
        {
            newSubject = animals[Random.Range(0, animals.Length)];
        }

        return newSubject;
    }

    public void SpawnPaper()
    {
        if (newSubject == null)
        {
            SetNewSubject();
        }

        GameObject newPaper = Instantiate(paperPrefab, otherPaperPosition.transform.position, Quaternion.identity, transform);
        newPaper.GetComponent<Paper>().InitializePaper(newSubject);
        newPaper.transform.localRotation = Quaternion.identity;
        if (papers.Count == 0) newPaper.GetComponent<Paper>().MoveToPosition(currentPaperPosition.transform.position);
        if (papers.Count > 1) newPaper.SetActive(false);
        papers.Add(newPaper);

        newSubject = null;
    }

    void ActualSpawnPaper(Subject newSubject)
    {
        GameObject newPaper = Instantiate(paperPrefab, otherPaperPosition.transform.position, Quaternion.identity, transform);
        newPaper.GetComponent<Paper>().InitializePaper(newSubject);
        newPaper.transform.localRotation = Quaternion.identity;
        if (papers.Count == 0) newPaper.GetComponent<Paper>().MoveToPosition(currentPaperPosition.transform.position);
        if (papers.Count > 1) newPaper.SetActive(false);
        papers.Add(newPaper);
    }

    private void Update()
    {
        if (!gameFinished)
        {
            if (papers.Count > 0 && !onCooldown)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    pikachuController.PassPaperLeft();
                    bool success = animalBox.ReceivePaper(papers[0]);
                    pikachuController.SetPassCorrectness(success);

                    StartCoroutine(ShufflePagePositions());
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    pikachuController.PassPaperRight();
                    bool success = humanBox.ReceivePaper(papers[0]);
                    pikachuController.SetPassCorrectness(success);

                    StartCoroutine(ShufflePagePositions());
                }
            }
        }
    }

    void StopCooldown()
    {
        onCooldown = false;
    }

    IEnumerator ShufflePagePositions()
    {
        papers.RemoveAt(0);
        onCooldown = true;
        Invoke(nameof(StopCooldown), 0.1f);
        if (papers.Count > 0) papers[0].GetComponent<Paper>().MoveToPosition(currentPaperPosition.transform.position);
        yield return new WaitForSeconds(0.5f);
        if (papers.Count > 1)
        {
            papers[1].SetActive(true);
        } 
    }

    public void WinGame()
    {
        gameFinished = true;
        pikachuController.Success();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ActionsController : MonoBehaviour
{
    public GameObject upPrefab;
    public GameObject leftPrefab;
    public GameObject downPrefab;
    public GameObject rightPrefab;
    public GameObject fPrefab;
    public GameObject gPrefab;

    public float scaleFactor = 1;

    Dictionary<Action, GameObject> actionPrefabs = new Dictionary<Action, GameObject>();
    List<GameObject> actions = new List<GameObject>();

    int currentActionIndex = 0;

    UnityEvent doneDisplayingSeqActions;

    float midpoint;
    int actionCount;

    private void Start()
    {
        actionPrefabs.Add(Action.Up, upPrefab);
        actionPrefabs.Add(Action.Left, leftPrefab);
        actionPrefabs.Add(Action.Down, downPrefab);
        actionPrefabs.Add(Action.Right, rightPrefab);
        actionPrefabs.Add(Action.F, fPrefab);
        actionPrefabs.Add(Action.G, gPrefab);

        midpoint = GetComponent<RectTransform>().rect.width / 2;

        if (doneDisplayingSeqActions == null)
            doneDisplayingSeqActions = new UnityEvent();
    }

    public void AddListenerToSeqActions(UnityAction action)
    {
        doneDisplayingSeqActions.AddListener(action);
    }

    public void DisplayActions(List<Action> actions)
    {
        GameObject actionObj;

        foreach (Action act in actions)
        {
            actionPrefabs.TryGetValue(act, out actionObj);

            if (actionObj) this.actions.Add(Instantiate(actionObj, transform));
        }
    }

    public void DisplayActionsSequentially(List<Action> actions, float waitTime)
    {
        //SetPlaceholders(actions.Count);
        StartCoroutine(DisplayActionsSeq(actions, waitTime));
    }

    public void SetPlaceholders(int numActions)
    {
        actionCount = numActions;
    }

    IEnumerator DisplayActionsSeq(List<Action> newActions, float waitTime)
    {
        yield return new WaitForSeconds(1f);

        float offset = GetComponent<RectTransform>().rect.width / newActions.Count;
        GameObject actionObj;

        for (int i = 0; i < newActions.Count; i++)
        {
            yield return new WaitForSeconds(waitTime);
            actionPrefabs.TryGetValue(newActions[i], out actionObj);

            if (actionObj)
            {
                actions.Add(Instantiate(actionObj, transform));
                actions[i].transform.localPosition = new Vector3(offset * (i + (i + 1f)) / 2f - midpoint, 0, 0);
                actions[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            } 
        }

        doneDisplayingSeqActions.Invoke();
    }

    public void ShowCorrectAction(Action action)
    {
        GameObject actionObj;
        actionPrefabs.TryGetValue(action, out actionObj);

        float offset = GetComponent<RectTransform>().rect.width / actionCount;

        actions.Add(Instantiate(actionObj, transform));
        actions[currentActionIndex].transform.localPosition = new Vector3(offset * (currentActionIndex + (currentActionIndex + 1f)) / 2f - midpoint, 0, 0);
        actions[currentActionIndex].transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        actions[currentActionIndex].GetComponent<Image>().color = Color.green;

        currentActionIndex++;
    }

    public void ShowIncorrectAction(Action action)
    {
        // Temporary as we use keyboard input
        GameObject actionObj;
        actionPrefabs.TryGetValue(action, out actionObj);

        float offset = GetComponent<RectTransform>().rect.width / actionCount;

        actions.Add(Instantiate(actionObj, transform));
        actions[currentActionIndex].transform.localPosition = new Vector3(offset * (currentActionIndex + (currentActionIndex + 1f)) / 2f - midpoint, 0, 0);
        actions[currentActionIndex].transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        actions[currentActionIndex].GetComponent<Image>().color = Color.red;
    }

    public void HideActions()
    {
        foreach (GameObject action in actions)
        {
            Destroy(action);
        }
        actions.Clear();
        currentActionIndex = 0;
    }
}

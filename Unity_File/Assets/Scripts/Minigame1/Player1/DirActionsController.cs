using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DirActionsController : MonoBehaviour
{
    public GameObject upPrefab;
    public GameObject leftPrefab;
    public GameObject downPrefab;
    public GameObject rightPrefab;

    Dictionary<Direction, GameObject> dirPrefabs = new Dictionary<Direction, GameObject>();
    List<GameObject> actions = new List<GameObject>();

    int currentActionIndex = 0;

    private void Start()
    {
        dirPrefabs.Add(Direction.Up, upPrefab);
        dirPrefabs.Add(Direction.Left, leftPrefab);
        dirPrefabs.Add(Direction.Down, downPrefab);
        dirPrefabs.Add(Direction.Right, rightPrefab);
    }

    public void DisplayActions(List<Direction> directions)
    {
        foreach (Direction dir in directions)
        {
            GameObject directionObj;
            dirPrefabs.TryGetValue(dir, out directionObj);

            if (directionObj) actions.Add(Instantiate(directionObj, transform));
        }
    }

    public void CompletedAction()
    {
        // Temporary as we use keyboard input
        actions[currentActionIndex].GetComponent<Image>().color = Color.green;

        currentActionIndex++;
    }

    public void IncorrectAction()
    {
        // Temporary as we use keyboard input
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

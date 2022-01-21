using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridValue { Space, Player, Box, Destination }

public class GridController : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject bottomLeft;
    public int height;
    public int width;
    public float gridSpacing;

    GridSpaceController[,] grid;

    public GameObject boxPrefab;
    BoxController box;
    int boxRow;
    int boxCol;

    int destRow; 
    int destCol;
    bool reachedDestination = false;

    [Header("SFX")]
    public AudioClip pushBoxClip;
    public AudioClip pullBoxClip;
    public AudioClip successClip;
    AudioSource audioSource;

    M2GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<M2GameManager>();
        audioSource = GetComponent<AudioSource>();

        grid = new GridSpaceController[height, width];

        CreateGrid();
        InitialSpawnBox();
        InitialSpawnDestination();
    }

    void CreateGrid()
    {
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                GameObject gridSpace = Instantiate(gridPrefab, transform);
                gridSpace.transform.position = bottomLeft.transform.position + (new Vector3(c * gridSpacing, 0, r * gridSpacing));
                grid[r, c] = gridSpace.GetComponent<GridSpaceController>();
            }
        }

        bottomLeft.SetActive(false);
    }

    void InitialSpawnBox()
    {
        boxRow = Random.Range(1, width);
        boxCol = Random.Range(1, height);

        CreateBox();
    }

    void InitialSpawnDestination()
    {
        do {
            destRow = Random.Range(1, width);
            destCol = Random.Range(1, height);
        } while (GetPositionValue(destRow, destCol) != GridValue.Space);

        CreateDestination();
    }

    void SpawnBox()
    {
        do {
            boxRow = Random.Range(0, width);
            boxCol = Random.Range(0, height);
        } while (GetPositionValue(boxRow, boxCol) != GridValue.Space);

        CreateBox();
    }

    void SpawnDestination()
    {
        do {
            destRow = Random.Range(0, width);
            destCol = Random.Range(0, height);
        } while (GetPositionValue(destRow, destCol) != GridValue.Space);

        CreateDestination();
    }

    void CreateBox()
    {
        GameObject boxObject = Instantiate(boxPrefab, transform);
        box = boxObject.GetComponent<BoxController>();
        box.InitializePosition(boxRow, boxCol);
        SetPositionValue(boxRow, boxCol, GridValue.Box);
    }

    void CreateDestination()
    {
        SetPositionValue(destRow, destCol, GridValue.Destination);
    }

    public void SetPositionValue(int row, int col, GridValue value)
    {
        grid[row, col].SetValue(value);
    }

    public GridValue GetPositionValue(int row, int col)
    {
        return grid[row, col].GetValue();
    }

    public Vector3 GetPosition(int row, int col)
    {
        return grid[row, col].gameObject.transform.position;
    }

    public bool IsValidPosition(int row, int col)
    {
        return 0 <= row && row < height && 0 <= col && col < width;
    }

    int GetXOffset(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                return -1;
            case Direction.Right:
                return 1;
        }
        return 0;
    }

    int GetYOffset(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return -1;
            case Direction.Up:
                return 1;
        }
        return 0;
    }

    public bool HasBoxInView(int pRow, int pCol, Direction dir)
    {
        if (reachedDestination) return false;
        int xOff = GetXOffset(dir);
        int yOff = GetYOffset(dir);

        int rowDiff = boxRow - pRow;
        int colDiff = boxCol - pCol;

        return (colDiff * xOff > 0 && yOff == 0 && rowDiff == 0) 
            || (xOff == 0 && colDiff == 0 && rowDiff * yOff > 0);
    }

    public void PushBox(Direction dir)
    {
        SetPositionValue(boxRow, boxCol, GridValue.Space);

        int xOff = GetXOffset(dir);
        int yOff = GetYOffset(dir);

        int currRow = boxRow + yOff;
        int currCol = boxCol + xOff;

        bool changedPos = false;

        while (IsValidPosition(currRow, currCol))
        {
            changedPos = true;
            boxRow = currRow;
            boxCol = currCol;

            if (GetPositionValue(currRow, currCol) == GridValue.Destination)
            {
                ReachedDestination();
                break;
            }

            currRow += yOff;
            currCol += xOff;
        }

        box.SetPosition(boxRow, boxCol);
        SetPositionValue(boxRow, boxCol, GridValue.Box);

        if (changedPos && !reachedDestination)
        {
            audioSource.clip = pushBoxClip;
            audioSource.Play();
        }
    }

    public void PullBox(Direction dir)
    {
        SetPositionValue(boxRow, boxCol, GridValue.Space);

        int xOff = GetXOffset(dir);
        int yOff = GetYOffset(dir);

        int currRow = boxRow - yOff;
        int currCol = boxCol - xOff;

        bool changedPos = false;

        while (IsValidPosition(currRow, currCol))
        {
            if (GetPositionValue(currRow, currCol) == GridValue.Player)
            {
                break;
            }

            changedPos = true;
            boxRow = currRow;
            boxCol = currCol;

            if (GetPositionValue(currRow, currCol) == GridValue.Destination)
            {
                ReachedDestination();
                break;
            }

            currRow -= yOff;
            currCol -= xOff;
        }

        box.SetPosition(boxRow, boxCol);
        SetPositionValue(boxRow, boxCol, GridValue.Box);

        if (changedPos && !reachedDestination)
        { 
            audioSource.clip = pullBoxClip;
            audioSource.Play();
        }
    }

    void ReachedDestination()
    {
        reachedDestination = true;

        grid[boxRow, boxCol].StartParticles();
        audioSource.clip = successClip;
        audioSource.Play();

        Invoke(nameof(ActuallyReachedDestination), 1f);
    }

    void ActuallyReachedDestination()
    {
        gameManager.AddToScore();
        reachedDestination = false;

        grid[boxRow, boxCol].StopParticles();
        Destroy(box.gameObject);

        SetPositionValue(boxRow, boxCol, GridValue.Space);
        SpawnBox();
        SpawnDestination();
    }
}

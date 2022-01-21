using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    GridController gridController;

    int boxRow;
    int boxCol;

    float moveSpeed = 5;
    Vector3 currentPos;
    Vector3 targetPos;
    bool isMoving = false;
    float currentTime;

    void Awake()
    {
        gridController = GameObject.Find("GridController").GetComponent<GridController>();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(currentPos, targetPos, currentTime);
            if (currentTime >= 1) isMoving = false;
            currentTime += Time.deltaTime * moveSpeed;
        }
    }

    public void InitializePosition(int row, int col)
    {
        boxRow = row;
        boxCol = col;

        Vector3 gridPos = gridController.GetPosition(boxRow, boxCol);
        gridPos.y = transform.position.y;
        transform.position = gridPos;
    }

    public void SetPosition(int row, int col)
    {
        boxRow = row;
        boxCol = col;

        Vector3 gridPos = gridController.GetPosition(boxRow, boxCol);
        gridPos.y = transform.position.y;

        currentPos = transform.position;
        targetPos = gridPos;
        currentTime = 0;
        isMoving = true;
    }
}

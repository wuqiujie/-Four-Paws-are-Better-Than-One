using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Paper : MonoBehaviour
{
    public TextMeshProUGUI descriptionText; // Used in place of art assets right now
    public Image image;
    Classification classification;

    float moveSpeed = 3;
    Vector3 currentPos;
    Vector3 targetPos;
    bool isMoving = false;
    float currentTime;

    public void InitializePaper(Subject subject)
    {
        descriptionText.text = subject.subjectName;
        image.sprite = subject.subjectPicture;
        classification = subject.subjectClassification;

        // Would also need to change art assets as needed
    }

    public Classification GetClassification()
    {
        return classification;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(currentPos, targetPos, currentTime);
            currentTime += Time.deltaTime * moveSpeed;
            if (currentTime >= 1) isMoving = false;
        }
    }

    public void MoveToPosition(Vector3 newPos)
    {
        currentPos = transform.position;
        targetPos = newPos;
        currentTime = 0;
        isMoving = true;
    }
}

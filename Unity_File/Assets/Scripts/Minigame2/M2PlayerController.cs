using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2PlayerController : MonoBehaviour
{
    GridController gridController;
    
    int playerRow = 0;
    int playerCol = 0;
    Direction playerDirection;

    bool keyDown;

    bool canMove = false;

    public Animator squirrelAnimator;
    public Animator pikachuAnimator;
    public LineRenderer grabLine;

    [Header("SFX")]
    public AudioClip walkClip;
    AudioSource audioSource;

    Vector3 currentPosition;
    Vector3 targetPosition;
    float currentTime;
    bool isMoving = false;

    void Start()
    {
        gridController = GameObject.Find("GridController").GetComponent<GridController>();
        gridController.SetPositionValue(playerRow, playerCol, GridValue.Player);
        grabLine.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    public void AllowMovement()
    {
        canMove = true;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, currentTime);
            currentTime += 3 * Time.deltaTime;

            if (currentTime >= 1)
            {
                squirrelAnimator.SetBool("IsWalking", false);
                transform.position = targetPosition;
                isMoving = false;
            }
        }

        if (!canMove) return;
        
        if (!keyDown)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                playerDirection = Direction.Up;
                TryMove(playerRow + 1, playerCol);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                playerDirection = Direction.Down;
                TryMove(playerRow - 1, playerCol);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
                playerDirection = Direction.Left;
                TryMove(playerRow, playerCol - 1);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                playerDirection = Direction.Right;
                TryMove(playerRow, playerCol + 1);
            }
        }

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) keyDown = false;

        if (Input.GetKeyDown(KeyCode.F))
        {
            pikachuAnimator.SetTrigger("HitLeft");

            // Check for box
            if (gridController.HasBoxInView(playerRow, playerCol, playerDirection))
            {
                //Pull box
                gridController.PullBox(playerDirection);
            }

            StartCoroutine(RenderGrabLine());
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            pikachuAnimator.SetTrigger("HitRight");

            // Check for box
            if (gridController.HasBoxInView(playerRow, playerCol, playerDirection))
            {
                //Push box
                gridController.PushBox(playerDirection);
            }

            StartCoroutine(RenderGrabLine());
        }
    }

    IEnumerator RenderGrabLine()
    {
        float currentTime = 0;
        float maxTime = 0.3f;
        LayerMask mask = LayerMask.GetMask("LaserHit");
        grabLine.gameObject.SetActive(true);
        grabLine.SetPosition(0, Vector3.zero);

        while (currentTime < maxTime)
        { 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
            {
                grabLine.SetPosition(1, Vector3.forward * hit.distance);
            }
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        grabLine.gameObject.SetActive(false);
    }

    void TryMove(int newRow, int newCol)
    {
        keyDown = true;
        audioSource.clip = walkClip;
        audioSource.Play();

        if (!gridController.IsValidPosition(newRow, newCol)) return;
        if (gridController.GetPositionValue(newRow, newCol) != GridValue.Space) return;

        gridController.SetPositionValue(playerRow, playerCol, GridValue.Space);
        gridController.SetPositionValue(newRow, newCol, GridValue.Player);

        Vector3 gridPos = gridController.GetPosition(newRow, newCol);
        gridPos.y = transform.position.y;

        squirrelAnimator.SetBool("IsWalking", true);
        targetPosition = gridPos;
        currentPosition = transform.position;
        currentTime = 0;
        isMoving = true;

        playerRow = newRow;
        playerCol = newCol;
    }

    public void EndGame()
    {
        canMove = false;
        squirrelAnimator.SetTrigger("Win");
        pikachuAnimator.SetTrigger("Win");
    }
}

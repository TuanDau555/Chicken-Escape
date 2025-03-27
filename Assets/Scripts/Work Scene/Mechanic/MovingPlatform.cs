using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Component
    PlayerController_WorkScene playerController;
    Rigidbody2D platformRb;

    [Header("Platform Statics")]
    [SerializeField] private float platformSpeed = 2f;
    [SerializeField] private float waitDuration = 0.3f;
    private Vector3 targetPos;
    private Vector3 platformMoveDirection;

    [Header("Platform Ways")]
    public GameObject ways;
    public Transform[] wayPoints;
    [SerializeField] private int pointIndex;
    [SerializeField] int pointCount;
    private int direction = 1; // there's alway 1 direction in a level
    #endregion

    #region MonoBehaviour Func
    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController_WorkScene>();
        platformRb = GetComponent<Rigidbody2D>();

        WayPoints();
    }

    /// <summary>
    /// pointIndex, wayPoints[1] are set to 1 because the Path is it
    /// </summary>
    private void Start()
    {
        pointIndex = 1; 
        pointCount = wayPoints.Length;
        targetPos = wayPoints[1].transform.position;

        DirectionCaculate();
    }

    private void Update()
    {
        PlatformDirection();
    }

    private void FixedUpdate()
    {
        PlatformMovement();
    }

    // To make the play be a part of the platform
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player is  on platform...
            playerController.isOnPlatform = true;
            // ...So player rigidbody be a part of the platform's rb
            playerController.platformRb = platformRb; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //... else turn player doesn't a part of platform anymore
            playerController.isOnPlatform = false; 
        }
    }
    #endregion

    #region Moving Plaform
    Vector3 PlatformMovement() => platformRb.velocity = platformMoveDirection * platformSpeed;

    void PlatformDirection()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        transform.position = targetPos;
        platformMoveDirection = Vector2.zero; // When function is called the platform won't move...

        if(pointIndex == pointCount - 1) //Arrived Last Point
        {
            direction = -1;
        }

        if(pointIndex == 0) //Arrived First Point
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;
        StartCoroutine(WaitNExtPoint()); // ...Now platform will move
    }

    IEnumerator WaitNExtPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCaculate();
    }

    /// <summary>
    /// if A(3,4) B(3,10) => AB(0,6) that mean direction is moving on Y-axis
    /// </summary>
    /// <returns>platformMoveDirection</returns>
    Vector3 DirectionCaculate() => platformMoveDirection = (targetPos - transform.position).normalized;

    void WayPoints()
    {
        wayPoints = new Transform[ways.transform.childCount]; // Get the Ways object and count it child
        for(int i = 0; i <ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform; // When counted get it position
        }
    }
    #endregion 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalfosMovement : EnemyMovement
{
    public float speed = 1f;
    public float activeInterval = 2f;
    public float triggerDistance = 4f;

    private Rigidbody rb;
    private GameObject player;
    private int active = 0;
    private FindWallDirection fwd;
    private Vector3 currentDirection = Utils.Direction.NONE;
    private GridMovement gm;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        fwd = GetComponent<FindWallDirection>();
        gm = GetComponent<GridMovement>();
    }

    void Update()
    {
        List<Vector3> wallList = fwd.FindWallList(Utils.Direction.FOUR_DIRECTIONS);
        if (wallList.Contains(currentDirection) || active == 0)
        {
            active -= 1;
            StartCoroutine(wakeUpAfterSeconds());
            StartCoroutine(move());
        }
    }

    IEnumerator move()
    {
        Vector3 nextDirection = currentDirection;
        List<Vector3> wallList = fwd.FindWallList(Utils.Direction.FOUR_DIRECTIONS);
        Vector3[] desiredDirections = new Vector3[] { };
        if ((player.transform.position - transform.position).magnitude <= triggerDistance)
        {
            desiredDirections = Utils.getDirectionFromCoordinates(player.transform.position - transform.position, Utils.Direction.FOUR_DIRECTIONS);
        }
        nextDirection = Utils.findAvailableDirection(desiredDirections, Utils.Direction.FOUR_DIRECTIONS, wallList.ToArray(), new Vector3[] { -currentDirection });
        yield return gm.MoveToClosestGrid(nextDirection.y == 0);
        currentDirection = nextDirection;
        rb.velocity = speed * currentDirection;
    }


    IEnumerator wakeUpAfterSeconds()
    {
        yield return new WaitForSeconds(activeInterval);
        active += 1;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(1, transform.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<Health>().immunTimeCountDown < 0f)
        {
            other.GetComponent<PlayerController>().takeDamage(1, other.transform.position);
        }
    }

    public override void initialize()
    {
        transform.position = originalPosition;
        active = 0;
    }

    public override void setActive()
    {
        active = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelMovement : EnemyMovement
{
    public float speed = 2f;
    public float triggerDistance = 4f;
    public float freezeProbability = 0.5f;
    public float freezeTime = 0.5f;

    private Rigidbody rb;
    private GameObject player;
    private bool active = true;
    private FindWallDirection fwd;
    private Vector3 currentDirection = Utils.Direction.NONE;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        fwd = GetComponent<FindWallDirection>();
    }

    void Update()
    {
        if (active)
        {
            float r = Random.Range(0, 100);
            if (r < 100 * freezeProbability)
            {
                StartCoroutine(freeze(freezeTime));
            }
            else
            {
                // raycasting for walls
                List<Vector3> wallList = fwd.FindWallList(Utils.Direction.FOUR_DIRECTIONS);

                Vector2 playerPosition = player.transform.position - transform.position;
                Vector3 targetPosition = transform.position;
                Vector3[] desiredDirection = playerPosition.magnitude > triggerDistance ? new Vector3[] { } :
                    Utils.getDirectionFromCoordinates(playerPosition, Utils.Direction.FOUR_DIRECTIONS);
                Vector3 direction = Utils.findAvailableDirection(desiredDirection, Utils.Direction.FOUR_DIRECTIONS, wallList.ToArray(), new Vector3[] { -currentDirection });
                currentDirection = direction;

                targetPosition = transform.position + direction;
                StartCoroutine(move(targetPosition));
            }
        }
    }

    IEnumerator move(Vector3 targetPosition)
    {
        active = false;
        yield return CoroutineUtilities.MoveObjectWithSpeed(transform, transform.position, targetPosition, speed);
        active = true;
    }

    IEnumerator freeze(float time)
    {
        active = false;
        yield return new WaitForSeconds(time);
        active = true;
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
        active = true;
    }

    public override void setActive()
    {
        active = true;
    }
}

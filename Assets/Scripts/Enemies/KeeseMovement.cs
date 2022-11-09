using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseMovement : EnemyMovement
{
    public float maxSpeed = 3f;
    public float flyTime = 6f;
    public float idleTime = 2f;
    public float decisionTime = 2f;

    private GameObject player;
    private Rigidbody rb;
    private bool active = true;
    private FindWallDirection fwd;
    private Animator animator;
    private float time;
    public float decisionTimeCount;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        fwd = GetComponent<FindWallDirection>();
        animator = GetComponent<Animator>();
        decisionTimeCount = 2.5f;
    }

    void Update()
    {
        time += Time.deltaTime;
        while (time >= flyTime + idleTime)
        {
            time -= (flyTime + idleTime);
        }
        decisionTimeCount += Time.deltaTime;

        if (time <= flyTime)
        {
            float speed = maxSpeed * Mathf.Sin(time / flyTime * Mathf.PI);
            if (decisionTimeCount >= decisionTime)
            {
                while (decisionTimeCount >= decisionTime)
                {
                    decisionTimeCount -= decisionTime;
                }
                List<Vector3> wallList = fwd.FindWallList(Utils.Direction.EIGHT_DIRECTIONS);
                rb.velocity = speed * Utils.findAvailableDirection(new Vector3[] { }, Utils.Direction.EIGHT_DIRECTIONS, wallList.ToArray(), new Vector3[] { });
            }
            else
            {
                rb.velocity = speed * rb.velocity.normalized;
            }
            animator.speed = speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.speed = 0f;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(0.5f, transform.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<Health>().immunTimeCountDown < 0f)
        {
            other.GetComponent<PlayerController>().takeDamage(0.5f, other.transform.position);
        }
    }

    public override void initialize()
    {
        transform.position = originalPosition;
        active = true;
        decisionTimeCount = 2.5f;
    }

    public override void setActive()
    {
        active = true;
    }
}

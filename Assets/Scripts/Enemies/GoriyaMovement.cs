using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoriyaMovement : EnemyMovement
{
    public float speed = 1f;
    public float throwProbability = 0f;
    public float activeInterval = 2f;
    public GameObject boomerang;

    private Rigidbody rb;
    public int active = 0;
    private GridMovement gm;
    private FindWallDirection fwd;
    private Vector3 currentDirection = Utils.Direction.UP;
    private bool throwing = false;
    private Animator animator;
    public GameObject selfBoomerang = null;
    private int triggerCount = 0;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GetComponent<GridMovement>();
        fwd = GetComponent<FindWallDirection>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> wallList = fwd.FindWallList(Utils.Direction.FOUR_DIRECTIONS);
        if (!throwing && (wallList.Contains(currentDirection) || active == 0))
        {
            active -= 1;
            StartCoroutine(move());
        }
    }

    IEnumerator move()
    {
        bool toThrow = false;
        float r = UnityEngine.Random.Range(0, 100);
        if (r < 100 * throwProbability)
        {
            toThrow = true;
            throwing = true;
        }
        else
        {
            active -= 1;
            StartCoroutine(wakeUpAfterSeconds());
        }
        Vector3 nextDirection = currentDirection;
        List<Vector3> wallList = fwd.FindWallList(Utils.Direction.FOUR_DIRECTIONS);
        nextDirection = Utils.findAvailableDirection(new Vector3[] { }, Utils.Direction.FOUR_DIRECTIONS, wallList.ToArray(), new Vector3[] { -currentDirection });
        yield return gm.MoveToClosestGrid(nextDirection.y == 0);
        animator.SetFloat("horizontal", nextDirection.x);
        animator.SetFloat("vertical", nextDirection.y);
        currentDirection = nextDirection;
        if (toThrow)
        {
            rb.isKinematic = true;
            selfBoomerang = Instantiate(boomerang, transform.position, Quaternion.identity);
            selfBoomerang.GetComponent<GoriyaBoomerangController>().setDirection(currentDirection);
        }
        active += 1;

        rb.velocity = speed * currentDirection;
    }

    IEnumerator wakeUpAfterSeconds()
    {
        yield return new WaitForSeconds(activeInterval);
        active += 1;
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, selfBoomerang))
        {
            triggerCount++;
            if (triggerCount == 2)
            {
                triggerCount = 0;
                Destroy(other.gameObject);
                selfBoomerang = null;
                rb.isKinematic = false;
                throwing = false;
            }
        }
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

    void OnDestroy()
    {
        if (selfBoomerang != null)
        {
            Destroy(selfBoomerang);
        }
    }

    public override void initialize()
    {
        selfBoomerang = null;
        throwing = false;
        transform.position = originalPosition;
        active = 0;
    }

    public override void setActive()
    {
        active = 0;
    }
}

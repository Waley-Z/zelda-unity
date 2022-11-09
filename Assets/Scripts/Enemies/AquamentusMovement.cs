using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusMovement : EnemyMovement
{
    public Vector3 moveDirection = new Vector3(1, 0, 0);
    public float speed = 1f;
    public float attackInterval = 3f;
    public GameObject fireball;
    public float fireballAngle = 20;
    public int fireballCount = 3;
    public float fireballSpeed = 3f;

    private Vector3 nextPosition;
    private bool active = true;
    private float timeSinceLastAttack = 0;
    private GameObject player;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        nextPosition = originalPosition + moveDirection;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (active)
        {
            if (Vector3.Dot(transform.position - originalPosition, moveDirection) > 0)
            {
                nextPosition = originalPosition - moveDirection;
            }
            else
            {
                nextPosition = originalPosition + moveDirection;
            }
            active = false;
            StartCoroutine(move());
        }
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackInterval)
        {
            timeSinceLastAttack = 0;
            attack();
        }
    }

    IEnumerator move()
    {
        yield return CoroutineUtilities.MoveObjectWithSpeed(transform, transform.position, nextPosition, speed);
        active = true;
    }

    void attack()
    {
        Vector3 direction = player.transform.position - transform.position;
        for (int i = 0; i < fireballCount; i++)
        {
            GameObject fb = Instantiate(fireball, transform.position, Quaternion.identity);
            fb.GetComponent<Rigidbody>().velocity = fireballSpeed * (Quaternion.AngleAxis(((fireballCount - 1) / 2 - i) * fireballAngle, Vector3.forward) * direction.normalized);
        }
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
        SoundManager.PlaySound(SoundManager.Sound.WallMasterAquamentus);
        transform.position = originalPosition;
        active = true;
    }

    public override void setActive()
    {
        active = true;
    }
}

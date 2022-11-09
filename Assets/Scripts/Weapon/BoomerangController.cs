using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    public float maxSpeed = 2f;
    public float slope = 0.05f;

    private Rigidbody rb;
    private GameObject player;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool flyBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        rb.velocity = maxSpeed * direction;
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        if (Mathf.Abs(currentSpeed - 0) < slope / 2)
        {
            flyBack = true;
        }
        if (flyBack)
        {
            direction = Vector3.Normalize(player.transform.position - transform.position);
            currentSpeed += slope;
        } else
        {
            currentSpeed -= slope;
        }
        rb.velocity = currentSpeed * direction;
        SoundManager.PlaySound(SoundManager.Sound.SwordSwipe, 0.35f); // same audio clip
    }

    public void setDirection(Vector3 _d)
    {
        direction = _d;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!flyBack && (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("BowRoom") || other.gameObject.CompareTag("Enemy")))
        {
            flyBack = true;
        }
        if (flyBack && other.gameObject.CompareTag("Player"))
        {
            reset();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (flyBack && other.gameObject.CompareTag("Player"))
        {
            reset();
        }
    }

    private void reset()
    {
        Destroy(gameObject);
        player.GetComponent<Boomerang>().resetBoomerang();
        player.GetComponent<Boomerang>().animate();
    }
}

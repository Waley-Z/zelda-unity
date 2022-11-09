using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaBoomerangController : MonoBehaviour
{
    public float maxSpeed = 2f;
    public float slope = 2f;

    private Rigidbody rb;
    public Vector3 direction = Vector3.zero;
    public float currentSpeed;
    public float time = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        time += Time.deltaTime;
        currentSpeed = maxSpeed * Mathf.Cos(time * slope);
        rb.velocity = currentSpeed * direction;
    }

    public void setDirection(Vector3 _d)
    {
        direction = _d;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            time = (Mathf.PI - time) / slope;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(1, transform.position);
        }
    }
}


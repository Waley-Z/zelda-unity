using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusFireball : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(1, transform.position);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}


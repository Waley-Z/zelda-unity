using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float knockTime = 0.1f;
    public float knockDistance = 2f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Knock(Vector3 direction)
    {
        StartCoroutine(KnockCoroutine(direction));
    }

    private IEnumerator KnockCoroutine(Vector3 direction)
    {
        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine(GetComponent<PlayerController>().freezeControl(knockTime));
        }
        rb.velocity = knockDistance / knockTime * direction;
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector3.zero;

        EnemyMovement em = GetComponent<EnemyMovement>();
        if (em != null)
        {
            em.setActive();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrapMovement : EnemyMovement
{
    [System.Serializable]
    public struct WatchInfo
    {
        public Vector3 watchDirection;
        public float distance;
    }

    public WatchInfo[] watchInfos;
    public float forwardSpeed = 3f;
    public float goBackSpeed = 2f;

    private bool active = true;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (active && originalPosition == transform.position)
        {
            foreach (WatchInfo info in watchInfos)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, info.watchDirection, out hit, info.distance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        StartCoroutine(move(info));
                        active = false;
                        break;
                    }
                }
            }
        }
    }

    IEnumerator move(WatchInfo info)
    {
        yield return CoroutineUtilities.MoveObjectWithSpeed(transform, transform.position, transform.position + info.distance * info.watchDirection, forwardSpeed);
        yield return CoroutineUtilities.MoveObjectWithSpeed(transform, transform.position, originalPosition, goBackSpeed);
        active = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(1, transform.position, false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<Health>().immunTimeCountDown < 0f)
        {
            other.GetComponent<PlayerController>().takeDamage(1, other.transform.position, false);
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

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    public float timeThreshold = 1.5f;
    public float transitionTime = 1.5f;
    public bool moveDown = false;
    public bool moveLeft = false;
    public bool moveUp = false;
    public bool moveRight = false;
    public GameObject doorToDestroy;

    private bool hasBeenPushed = false;
    private float collisionStartTime = 0f;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void reset()
    {
        transform.position = originalPosition;
        hasBeenPushed = false;
        collisionStartTime = 0f;
    }

    private void OnCollisionEnter(Collision other)
    {

        if (isValidPush(other)) {
            collisionStartTime = Time.time;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (!isValidPush(other))
        {
            return;
        }

        if (collisionStartTime != 0f && Time.time - collisionStartTime > timeThreshold)
        {
            Vector3 faceDirection = other.gameObject.GetComponent<PlayerController>().faceDirection;
            Vector3 destPosition = transform.position + faceDirection;
            Vector3 otherPosition = other.transform.position;
            Vector3 otherDestPosition = otherPosition + faceDirection;
            if (moveDown && faceDirection == Utils.Direction.DOWN)
            {
            } 
            else if (moveUp && faceDirection == Utils.Direction.UP)
            {;
            }
            else if (moveLeft && faceDirection == Utils.Direction.LEFT)
            {
            }
            else if (moveRight && faceDirection == Utils.Direction.RIGHT)
            {
            }
            else
            {
                return;
            }


            hasBeenPushed = true;
            StartCoroutine(CoroutineUtilities.MoveObjectWithTime(transform, transform.position, destPosition, transitionTime));
            StartCoroutine(other.gameObject.GetComponent<PlayerController>().freezeControl(transitionTime));
            other.gameObject.GetComponent<PlayerController>().animator.speed = 1.0f;
            StartCoroutine(CoroutineUtilities.MoveObjectWithTime(other.transform, otherPosition, otherDestPosition, transitionTime));
            if (doorToDestroy != null)
            {
                StartCoroutine(OpenDoor());
            } else
            {
                SoundManager.PlaySound(SoundManager.Sound.OldManDoorOpen);
            }

        }
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(transitionTime);
        SoundManager.PlaySound(SoundManager.Sound.DoorOpen);
        SoundManager.PlaySound(SoundManager.Sound.OldManDoorOpen);

        doorToDestroy.SetActive(false);
    }

    private void OnCollisionExit(Collision other)
    {
        collisionStartTime = 0f;
    }

    private bool isValidPush(Collision other)
    {
        return !hasBeenPushed && other.gameObject.CompareTag("Player");
    }
}

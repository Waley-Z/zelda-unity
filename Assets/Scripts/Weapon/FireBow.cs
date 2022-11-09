using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBow : Weapon
{
    public GameObject fireBowLeftPrefab;
    public GameObject fireBowRightPrefab;
    public GameObject fireBowUpPrefab;
    public GameObject fireBowDownPrefab;
    public float speed = 10f;
    public float animationDuration = 0.25f;

    private Inventory inventory;
    private PlayerController playerController;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
    }

    public override void attack(Vector3 faceDirection)
    {
        if (!inventory.useRufee(1))
        {
            return;
        }

        animate();
        SoundManager.PlaySound(SoundManager.Sound.SwordSwipe, 0.35f); // same audio clip

        // generate a flying fireBow
        Vector3 fireBowPosition = transform.position;
        GameObject fireBowPrefab = fireBowDownPrefab;
        Vector3 fireBowVelocity = Vector3.zero;

        // determine where the fireBow flies according to the player's facing direction
        if (playerController.faceDirection == Utils.Direction.RIGHT)
        {
            fireBowPosition = new Vector3(fireBowPosition.x + 0.345f, fireBowPosition.y - 0.035f, fireBowPosition.z);
            fireBowPrefab = fireBowRightPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.LEFT)
        {
            fireBowPosition = new Vector3(fireBowPosition.x - 0.345f, fireBowPosition.y - 0.035f, fireBowPosition.z);
            fireBowPrefab = fireBowLeftPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.UP)
        {
            fireBowPosition = new Vector3(fireBowPosition.x - 0.035f, fireBowPosition.y + 0.345f, fireBowPosition.z);
            fireBowPrefab = fireBowUpPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.DOWN)
        {
            fireBowPosition = new Vector3(fireBowPosition.x + 0.035f, fireBowPosition.y - 0.345f, fireBowPosition.z);
            fireBowPrefab = fireBowDownPrefab;
        }

        // generate flying fireBow
        GameObject flyingfireBow = Instantiate(fireBowPrefab, fireBowPosition, Quaternion.identity);
        flyingfireBow.GetComponent<Rigidbody>().velocity = speed * playerController.faceDirection;
    }

    public void animate()
    {
        StartCoroutine(altWeaponAnimation(gameObject, animationDuration));
    }
}

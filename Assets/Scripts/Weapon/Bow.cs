using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public GameObject bowLeftPrefab;
    public GameObject bowRightPrefab;
    public GameObject bowUpPrefab;
    public GameObject bowDownPrefab;
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

        // generate a flying bow
        Vector3 bowPosition = transform.position;
        GameObject bowPrefab = bowDownPrefab;
        Vector3 bowVelocity = Vector3.zero;

        // determine where the bow flies according to the player's facing direction
        if (playerController.faceDirection == Utils.Direction.RIGHT)
        {
            bowPosition = new Vector3(bowPosition.x + 0.345f, bowPosition.y - 0.035f, bowPosition.z);
            bowPrefab = bowRightPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.LEFT)
        {
            bowPosition = new Vector3(bowPosition.x - 0.345f, bowPosition.y - 0.035f, bowPosition.z);
            bowPrefab = bowLeftPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.UP)
        {
            bowPosition = new Vector3(bowPosition.x - 0.035f, bowPosition.y + 0.345f, bowPosition.z);
            bowPrefab = bowUpPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.DOWN)
        {
            bowPosition = new Vector3(bowPosition.x + 0.035f, bowPosition.y - 0.345f, bowPosition.z);
            bowPrefab = bowDownPrefab;
        }

        // generate flying bow
        GameObject flyingbow = Instantiate(bowPrefab, bowPosition, Quaternion.identity);
        flyingbow.GetComponent<Rigidbody>().velocity = speed * playerController.faceDirection;
    }

    public void animate()
    {
        StartCoroutine(altWeaponAnimation(gameObject, animationDuration));
    }
}

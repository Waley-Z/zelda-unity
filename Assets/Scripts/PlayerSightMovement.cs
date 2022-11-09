using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSightMovement : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        if (playerController.faceDirection == Utils.Direction.UP)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (playerController.faceDirection == Utils.Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (playerController.faceDirection == Utils.Direction.DOWN)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (playerController.faceDirection == Utils.Direction.RIGHT)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

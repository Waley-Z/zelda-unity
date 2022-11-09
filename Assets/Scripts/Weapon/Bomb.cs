using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public GameObject bombPrefab;

    private Rigidbody rb;
    private Inventory inventory;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
    }
    public override void attack(Vector3 faceDirection)
    {
        if (!inventory.useBomb(1))
        {
            return;
        }

        Vector3 bombPosition = transform.position + playerController.faceDirection;

        // generate a bomb
        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
    }
}

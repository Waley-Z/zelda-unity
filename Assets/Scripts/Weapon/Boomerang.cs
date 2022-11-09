using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    public GameObject boomerangPrefab;
    public float animationDuration = 0.25f;

    private PlayerController playerController;
    private bool hasBoomerang = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public override void attack(Vector3 faceDirection)
    {
        if (hasBoomerang)
            return;
        hasBoomerang = true;
        Vector3 boomerangPosition = transform.position + playerController.faceDirection * 0.035f;

        animate();
        // generate flying boomerang
        GameObject flyingBoomerang = Instantiate(boomerangPrefab, boomerangPosition, Quaternion.identity);
        flyingBoomerang.GetComponent<BoomerangController>().setDirection(playerController.faceDirection);
    }

    public void resetBoomerang()
    {
        hasBoomerang = false;
    }

    public void animate()
    {
        StartCoroutine(altWeaponAnimation(gameObject, animationDuration));
    }
}

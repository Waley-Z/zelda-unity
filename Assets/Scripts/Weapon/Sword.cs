using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public float swordAnimationTime = 1f;
    public GameObject swordLeftPrefab;
    public GameObject swordRightPrefab;
    public GameObject swordUpPrefab;
    public GameObject swordDownPrefab;
    public float flyingSpeed = 10f;

    private Rigidbody rb;
    private Animator animator;
    private Health health;
    private PlayerController playerController;
    private bool hasFlyingSword = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        playerController = GetComponent<PlayerController>();
    }

    public override void attack(Vector3 faceDirection)
    {
        GameObject sword = generateSword();
        if (sword == null)
            Debug.LogError("Sword not generated!");
        StartCoroutine(swordAttackCoroutine(sword));
    }


    private IEnumerator swordAttackCoroutine(GameObject sword)
    {

        SoundManager.PlaySound(SoundManager.Sound.SwordSwipe);

        // set animator to sword
        animator.SetBool("swordAttack", true);
        animator.speed = 1.0f;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        yield return GetComponent<PlayerController>().freezeControl(swordAnimationTime);
        rb.isKinematic = false;
        if (sword != null)
        {
            sword.GetComponent<WeaponDealDamage>().setDestoyOnEnemy(true);
            if (health.fullHealth() && !hasFlyingSword)
            {
                SoundManager.PlaySound(SoundManager.Sound.SwordFire);

                hasFlyingSword = true;
                sword.GetComponent<Rigidbody>().velocity = flyingSpeed * playerController.faceDirection;
                sword.GetComponent<OnDestroyListener>().destoryEvents += () => { hasFlyingSword = false; };
            }
            else
            {
                Destroy(sword);
            }
        }
        animator.SetBool("swordAttack", false);
    }

    private GameObject generateSword()
    {
        // generate a flying sword
        Vector3 swordPosition = transform.position;
        GameObject swordPrefab = swordDownPrefab;

        // determine where the sword flies according to the player's facing direction
        if (playerController.faceDirection == Utils.Direction.RIGHT)
        {
            swordPosition = new Vector3(swordPosition.x + 0.715f, swordPosition.y - 0.062f, swordPosition.z);
            swordPrefab = swordRightPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.LEFT)
        {
            swordPosition = new Vector3(swordPosition.x - 0.722f, swordPosition.y - 0.063f, swordPosition.z);
            swordPrefab = swordLeftPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.UP)
        {
            swordPosition = new Vector3(swordPosition.x - 0.0935f, swordPosition.y + 0.748f, swordPosition.z);
            swordPrefab = swordUpPrefab;
        }
        else if (playerController.faceDirection == Utils.Direction.DOWN)
        {
            swordPosition = new Vector3(swordPosition.x + 0.035f, swordPosition.y - 0.645f, swordPosition.z);
            swordPrefab = swordDownPrefab;
        }
        GameObject sword = Instantiate(swordPrefab, swordPosition, Quaternion.identity);
        sword.GetComponent<WeaponDealDamage>().setDestoyOnEnemy(false);
        return sword;
    }
}

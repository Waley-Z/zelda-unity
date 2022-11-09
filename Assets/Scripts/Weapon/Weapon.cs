using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    abstract public void attack(Vector3 faceDirection);

    protected IEnumerator altWeaponAnimation(GameObject player, float duration)
    {
        Animator animator = player.GetComponent<Animator>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        PlayerController pc = player.GetComponent<PlayerController>();

        animator.SetBool("altWeaponAttack", true);
        rb.isKinematic = true;
        pc.active = false;
        yield return player.GetComponent<PlayerController>().freezeControl(duration);
        pc.active = true;
        rb.isKinematic = false;
        animator.SetBool("altWeaponAttack", false);
    }
}

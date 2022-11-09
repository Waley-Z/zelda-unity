using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDealDamage : MonoBehaviour
{
    public bool destroyOnEnemy = true;
    public bool destroyOnWall = true;
    public bool destroyAnimation = false;
    public float damage = 1f;

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("wall") || other.CompareTag("BowRoom") || other.CompareTag("Door")) && destroyOnWall)
        {
            destroyObject();
        }
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Health>() != null)
            {
                SoundManager.PlaySound(SoundManager.Sound.EnemyDead, 0.5f);
                if (destroyOnEnemy)
                {
                    destroyObject();
                }
                other.GetComponent<Health>().takeDamage(damage, transform.position);
            }
        }
    }

    public void setDestoyOnEnemy(bool b)
    {
        destroyOnEnemy = b;
    }

    private void destroyObject()
    {
        if (destroyAnimation)
        {
            Vector3 position = transform.position;
            foreach (Vector3 d in Utils.Direction.FOUR_OTHER_DIRECTIONS)
            {
                GameObject go = Instantiate(GameAssets.GetPrefab("sword_flash"), position, Quaternion.identity);
                go.GetComponent<SwordFlashController>().setDirection(d);
            }
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class Collector : MonoBehaviour
{
    private Inventory inventory;
    private Health health;
    // private List<GameObject> doorsCouldOpen = new List<GameObject>();


    void Start()
    {
        inventory = GetComponent<Inventory>();
        health = GetComponent<Health>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("rupee"))
        {
            SoundManager.PlaySound(SoundManager.Sound.RupeeCollect);
            if (inventory != null)
            {
                inventory.AddRufee(1);
            }
            Debug.Log("Collect rupee");
        }
        else if (other.gameObject.CompareTag("Heart"))
        {
            SoundManager.PlaySound(SoundManager.Sound.HeartCollect);
            health.restoreHealth(1);
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            SoundManager.PlaySound(SoundManager.Sound.KeyCollect);
            inventory.AddKey(1);
        }
        else if (other.gameObject.CompareTag("bomb"))
        {
            SoundManager.PlaySound(SoundManager.Sound.KeyCollect); // to be found
            inventory.AddBomb(1);
        }
        else if (other.gameObject.CompareTag("Bow"))
        {
            Debug.Log("bow added");
            StartCoroutine(CollectBow(other));
        }
        else if (other.gameObject.CompareTag("FireBow"))
        {
            Debug.Log("fire bow added");
            StartCoroutine(CollectFireBow(other));
        }
        else if (other.gameObject.CompareTag("HealthHeart"))
        {
            health.maxHealth += 1;
            health.restoreHealth(health.maxHealth);
        }
        else if (other.gameObject.CompareTag("Triforce"))
        {
            GetComponent<PlayerController>().WinCheer();
            other.gameObject.transform.position = transform.position + new Vector3(0, 0.8f, 0);
            StartCoroutine(GetComponent<PlayerController>().WinCheer());
            return;
        }
        else
        {
            return;
        }
        Destroy(other.gameObject);
    }
    IEnumerator CollectBow(Collider other)
    {
        float cheerTime = 3.0f;
        SoundManager.pauseBGM();
        SoundManager.PlaySound(SoundManager.Sound.WeaponCollect);
        SoundManager.PlaySound(SoundManager.Sound.OldManRoomEnter);
        GetComponent<PlayerController>().addAltWeapon(PlayerController.AltWeapon.Bow, true, cheerTime);
        yield return new WaitForSeconds(cheerTime);
        SoundManager.playBGM();
    }
    IEnumerator CollectFireBow(Collider other)
    {
        float cheerTime = 3.0f;
        SoundManager.pauseBGM();
        SoundManager.PlaySound(SoundManager.Sound.WeaponCollect);
        SoundManager.PlaySound(SoundManager.Sound.OldManRoomEnter);
        GetComponent<PlayerController>().addAltWeapon(PlayerController.AltWeapon.FireBow, true, cheerTime);
        yield return new WaitForSeconds(cheerTime);
        SoundManager.playBGM();
    }
}


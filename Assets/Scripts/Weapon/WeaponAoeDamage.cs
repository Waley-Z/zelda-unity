using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAoeDamage : MonoBehaviour
{
    public float countdown = 1f;
    public float aoeRadius = 2f;
    public GameObject explosionPrefab;

    void Start()
    {
        StartCoroutine(StartBomb());
    }

    private IEnumerator StartBomb()
    {
        yield return new WaitForSeconds(countdown);

        Debug.Log("Boom");

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);


        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gos)
        {
            float distance = Vector3.Distance(go.transform.position, transform.position);
            if (distance <= aoeRadius)
            {
                go.GetComponent<Health>().takeDamage(2, transform.position);
                SoundManager.PlaySound(SoundManager.Sound.EnemyDead, 0.5f);
            }
        }
        Destroy(gameObject);
    }
}

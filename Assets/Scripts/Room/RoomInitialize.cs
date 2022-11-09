using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInitialize : MonoBehaviour
{
    private GameObject prefab;

    void Awake()
    {
        leaveRoom();
        if (GameObject.ReferenceEquals(GameObject.FindWithTag("Player").GetComponent<PlayerController>().firstRoom.gameObject, gameObject))
        {
            Debug.Log("initialize");
            enterRoom();
            Camera.main.transform.position = transform.position + new Vector3(0, 2, -1);
        }
    }

    void Start()
    {
        prefab = GameAssets.GetPrefab("spawn");
    }

    public void enterRoom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            StartCoroutine(enemySpawn(t));
        }
    }

    private IEnumerator enemySpawn(Transform t)
    {
        EnemyMovement em = t.GetComponent<EnemyMovement>();
        if (em != null && t.GetComponent<WallMasterMovement>() == null)
        {
            Instantiate(prefab, em.originalPosition != Vector3.zero ? em.originalPosition : t.position, Quaternion.identity);
            Debug.Log(em.originalPosition);
            yield return new WaitForSeconds(prefab.GetComponent<SpawnAnim>().animationDuration);
        }
        t.gameObject.SetActive(true);
        if (em != null)
        {
            em.initialize();
        }
    }

    public void leaveRoom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.gameObject.SetActive(false);
        }
    }
}

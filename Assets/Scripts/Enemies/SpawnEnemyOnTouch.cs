using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyOnTouch : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnEnemy
    {
        public GameObject enemy;
        public Vector3 spawnLocation;
        public Vector3 wallMasterActivateDirection;
        public Vector3 wallMasterMoveDirection;
    }

    public List<SpawnEnemy> spawnEnemies = new List<SpawnEnemy>();


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            foreach (SpawnEnemy e in spawnEnemies)
            {
                GameObject go = Instantiate(e.enemy, transform.position + e.spawnLocation, Quaternion.identity);
                if (go.GetComponent<WallMasterMovement>() != null)
                {
                    go.GetComponent<WallMasterMovement>().activateDirection = e.wallMasterActivateDirection;
                    go.GetComponent<WallMasterMovement>().moveDirection = e.wallMasterMoveDirection;
                }
                if (go.GetComponent<DeathDrop>() != null)
                {
                    for (int i = 0; i < go.GetComponent<DeathDrop>().objects.Length; i++)
                    {
                        go.GetComponent<DeathDrop>().objects[i].probability = 0f;
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }
}

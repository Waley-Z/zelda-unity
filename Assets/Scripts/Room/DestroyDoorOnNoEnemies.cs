using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDoorOnNoEnemies : MonoBehaviour
{
    public GameObject room;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (room.transform.childCount == 0)
        {
            Destroy(gameObject);
            SoundManager.PlaySound(SoundManager.Sound.DoorOpen);
        }
    }
}

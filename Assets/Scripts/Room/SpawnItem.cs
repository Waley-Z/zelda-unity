using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject itemPrefab;
    public Vector3 itemPosition = new Vector3(0, 0, 0);

    private bool itemDropped = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0 && !itemDropped)
        {
            Instantiate(itemPrefab, itemPosition, Quaternion.identity);
            SoundManager.PlaySound(SoundManager.Sound.KeyDrop, 0.6f);
            itemDropped = true;
        }
    }
}

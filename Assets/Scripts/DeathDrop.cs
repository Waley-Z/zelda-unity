using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    [System.Serializable]
    public struct DeathDropObjects
    {
        public float probability;
        public GameObject dropObject;
    }

    public DeathDropObjects[] objects;
    public Vector3 dropDirection = Vector3.zero;

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        foreach (DeathDropObjects obj in objects)
        {
            int r = UnityEngine.Random.Range(0, 100);
            if (obj.probability * 100 > r)
            {
                Instantiate(obj.dropObject, transform.position + dropDirection, Quaternion.identity);
                return;
            }
        }
    }
}

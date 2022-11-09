using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyListener : MonoBehaviour
{
    public System.Action destoryEvents;

    void OnDestroy()
    {
        if (destoryEvents != null)
        {
            destoryEvents();
        }
    }
}

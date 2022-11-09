using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyMovement : MonoBehaviour
{
    public Vector3 originalPosition;

    abstract public void initialize();

    abstract public void setActive();
}

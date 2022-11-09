using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float speed = 10f;
    public int gridPerUnit = 2;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator MoveToClosestGrid(bool horizontal)
    {
        // grid movement: start a coroutine to move to closest grid
        Vector3 position = transform.position;
        if (!horizontal)
        {
            float x = transform.position.x;
            float lower, upper;
            findLowerAndUpperGrid(x, out lower, out upper);
            float rightDist = upper - x;
            float leftDist = x - lower;
            bool right = rightDist < leftDist;
            Vector3 newPosition = new Vector3(right ? upper : lower, position.y, position.z);
            //if (animator != null)
            //{
            //    animator.SetFloat("horizontal", (newPosition - transform.position).x);
            //    animator.SetFloat("vertical", (newPosition - transform.position).y);
            //}
            yield return StartCoroutine(CoroutineUtilities.MoveObjectWithSpeed(transform, transform.position, newPosition, speed));
        }
        else
        {
            float y = transform.position.y;
            float lower, upper;
            findLowerAndUpperGrid(y, out lower, out upper);
            float upDist = upper - y;
            float downDist = y - lower;
            bool up = upDist < downDist;
            Vector3 newPosition = new Vector3(position.x, up ? upper : lower, position.z);
            //if (animator != null)
            //{
            //    animator.SetFloat("horizontal", (newPosition - transform.position).x);
            //    animator.SetFloat("vertical", (newPosition - transform.position).y);
            //}
            yield return StartCoroutine(CoroutineUtilities.MoveObjectWithSpeed(transform, transform.position, newPosition, speed));
        }
    }

    void findLowerAndUpperGrid(float pos, out float lower, out float upper)
    {
        // find the lower and upper of the grid at pos given that a unit is divided into gridPerUnit grids
        float floor = Mathf.Floor(pos);
        lower = floor + Mathf.Floor((pos - floor) * gridPerUnit) / gridPerUnit;
        upper = floor + Mathf.Floor((pos - floor) * gridPerUnit + 1) / gridPerUnit;
    }
}

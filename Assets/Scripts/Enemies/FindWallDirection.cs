using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWallDirection : MonoBehaviour
{
    public float rayLength = 0.8f;
    public List<Vector3> FindWallList(Vector3[] directions)
    {
        // raycasting for walls
        List<Vector3> wallList = new List<Vector3>();

        foreach (Vector3 dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, rayLength))
            {
                if (hit.transform.CompareTag("wall") || hit.transform.CompareTag("InnerWall") || hit.transform.CompareTag("Door"))
                {
                    wallList.Add(dir);
                }
            }
        }
        return wallList;
    }
}

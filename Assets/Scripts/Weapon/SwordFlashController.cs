using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlashController : MonoBehaviour
{
    public float animationDuration = .8f;
    public float animationSpeed = 1;


    private Vector3 direction;
    private float time = 0;
    private Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.position = startPosition + direction * animationSpeed * time;
        if (time > animationDuration)
            Destroy(gameObject);
    }

    public void setDirection(Vector3 d)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (d == Utils.Direction.DOWN_LEFT)
        {
            spriteRenderer.flipY = true;
        } else if (d == Utils.Direction.DOWN_RIGHT)
        {
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = true;
        }
        else if (d == Utils.Direction.UP_RIGHT)
        {
            spriteRenderer.flipX = true;
        }
        direction = d;
    }
}

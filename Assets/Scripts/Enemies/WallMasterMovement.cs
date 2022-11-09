using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMasterMovement : EnemyMovement
{
    public float speed = 1f;
    public Vector3 activateDirection;
    public Vector3 moveDirection;
    public bool triggerByDistance = true;
    public float visibleDistance = 3f;
    public WallMasterMovement nextToTrigger;

    private GameObject player;
    public bool visible = false;
    private bool capturePlayer = false;
    private Rigidbody rb;
    private int currentStage = 0;
    private PlayerController playerController;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (visible)
        {
            if (currentStage == 0)
            {
                if ((transform.position - originalPosition).magnitude > activateDirection.magnitude)
                {
                    currentStage++;
                    rb.velocity = moveDirection.normalized * speed;
                }
            }
            else if (currentStage == 1)
            {
                if ((transform.position - originalPosition).magnitude > (activateDirection + moveDirection).magnitude)
                {
                    currentStage++;
                    rb.velocity = -activateDirection.normalized * speed;
                }
            }
            else
            {
                if (Vector3.Dot(transform.position - (originalPosition + moveDirection), activateDirection) <= 0)
                {
                    currentStage++;
                    rb.velocity = Vector3.zero;
                    if (capturePlayer)
                    {
                        capturePlayer = false;
                        player.transform.position = player.GetComponent<PlayerController>().originalPosition;
                        playerController.currentRoom.leaveRoom();
                        Camera.main.transform.position = Camera.main.transform.position + (playerController.firstRoom.transform.position - playerController.currentRoom.transform.position);
                        playerController.active = true;
                    }
                    triggerNext();
                    return;
                }
            }
        }
        else if (triggerByDistance && !visible && (player.transform.position - transform.position).magnitude < visibleDistance)
        {
            triggerVisible();
        }
        if (capturePlayer)
        {
            player.transform.position = transform.position;
        }
    }

    void triggerVisible()
    {
        Debug.Log("trigger visible");
        SoundManager.PlaySound(SoundManager.Sound.WallMasterAquamentus);
        visible = true;
        if (rb != null)
        {
            rb.velocity = activateDirection.normalized * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.active = false;
            capturePlayer = true;
        }
    }

    void OnDestroy()
    {
    }

    public void triggerNext()
    {
        if (gameObject.activeSelf && nextToTrigger != null && nextToTrigger.enabled && !nextToTrigger.visible)
        {
            nextToTrigger.triggerVisible();
        }
    }

    public override void initialize()
    {
        transform.position = originalPosition;
        visible = false;
        capturePlayer = false;
        currentStage = 0;
    }

    public override void setActive()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public GameObject[] rooms = new GameObject[2];
    public float cameraSpeed = 5f;
    public bool isOldManRoom = false;
    public GameObject pushableBlockToReset;
    public GameObject doorToShut;

    private GameObject player;
    private PlayerController playerController;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerController.active)
        {
            Debug.Log("trigger enter" + gameObject.name);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerController.active = false;
            StartCoroutine(transit(other));
        }
    }

    IEnumerator transit(Collider other)
    {
        playerController.active = false;

        // find where the player enters
        int closerRoomIndex = 0;
        if ((rooms[1].transform.position - player.transform.position).magnitude < (rooms[0].transform.position - player.transform.position).magnitude)
        {
            closerRoomIndex = 1;
        }
        GameObject prevRoom = rooms[closerRoomIndex];
        GameObject nextRoom = rooms[(closerRoomIndex + 1) % 2];
        playerController.currentRoom = nextRoom.GetComponent<RoomInitialize>();

        // move player to center
        yield return CoroutineUtilities.MoveObjectWithSpeed(player.transform, player.transform.position, transform.position, playerController.speed);

        // Debug.Log(player.transform.position);
        // Debug.Log(transform.position);
        // move camera
        Vector3 cameraDest = Camera.main.transform.position + nextRoom.transform.position - prevRoom.transform.position;
        yield return CoroutineUtilities.MoveObjectWithSpeed(Camera.main.transform, Camera.main.transform.position, cameraDest, cameraSpeed);


        prevRoom.GetComponent<RoomInitialize>().leaveRoom();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("EnemyWeapon"))
        {
            Destroy(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (g.transform.parent == null)
            {
                Destroy(g);
            }
        }

        // move player
        Vector3 playerDest = transform.position + (nextRoom.transform.position - transform.position).normalized * 2.5f;
        yield return CoroutineUtilities.MoveObjectWithSpeed(player.transform, player.transform.position, playerDest, playerController.speed);
        nextRoom.GetComponent<RoomInitialize>().enterRoom();

        // old man room after in
        if (isOldManRoom && closerRoomIndex == 1)
        {
            SoundManager.PlaySound(SoundManager.Sound.OldManRoomEnter);
            yield return new WaitForSeconds(0.5f);
            playerController.active = true;
            if (GameObject.Find("Old Man Room Text") == null)
            {
                Debug.Log("error");
            }
            StartCoroutine(GameObject.Find("Old Man Room Text").GetComponent<Typewriter>().TypeWriterTMP());
            pushableBlockToReset.GetComponent<PushableBlock>().reset();
        }

        // old man room after out
        if (isOldManRoom && closerRoomIndex == 0)
        {
            GameObject.Find("Old Man Room Text").GetComponent<Typewriter>().reset();
        }

        playerController.active = true;

        if (doorToShut != null && closerRoomIndex == 0)
        {
            SoundManager.PlaySound(SoundManager.Sound.DoorOpen);
            doorToShut.SetActive(true);
        }
    }
}

using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomTransitionBow : MonoBehaviour
{
    public GameObject[] rooms = new GameObject[2];
    public GameObject[] teleportPosition = new GameObject[2];
    public Image blackPanel;
    public List<Color> panelColors = new List<Color> { new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 0.25f), new Color(0f, 0f, 0f, 0.5f), new Color(0f, 0f, 0f, 0.75f), new Color(0f, 0f, 0f, 1f)};
    public float panelChangeTime = 0.15f;
    public GameObject pushableBlockToReset;

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
        if ((teleportPosition[1].transform.position - player.transform.position).magnitude < (teleportPosition[0].transform.position - player.transform.position).magnitude)
        {
            closerRoomIndex = 1;
        }

        GameObject prevRoom = rooms[closerRoomIndex];
        GameObject nextRoom = rooms[(closerRoomIndex + 1) % 2];
        GameObject prevPosition = teleportPosition[closerRoomIndex];
        GameObject nextPosition = teleportPosition[(closerRoomIndex + 1) % 2];

        player.transform.position = new Vector3(10000f, 0, 0);
        prevRoom.GetComponent<RoomInitialize>().leaveRoom();

        for(int i = 0; i < panelColors.Count; i++)
        {
            blackPanel.color = panelColors[i];
            yield return new WaitForSeconds(panelChangeTime);
        }

        Camera.main.transform.position += (nextRoom.transform.position - prevRoom.transform.position);

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("EnemyWeapon"))
        {
            Destroy(g);
        }

        for (int i = panelColors.Count - 1; i >= 0; i--)
        {
            blackPanel.color = panelColors[i];
            yield return new WaitForSeconds(panelChangeTime);
        }

        nextRoom.GetComponent<RoomInitialize>().enterRoom();

        // out of bow room
        if (closerRoomIndex == 0)
        {
            player.transform.position = nextPosition.transform.position + new Vector3(-2.5f, -2, 0);
        }
        // into bow room
        else if (closerRoomIndex == 1)
        {
            player.transform.position = nextPosition.transform.position;

            Vector3 playerDirection = new Vector3(0, -1, 0);
            player.GetComponent<Animator>().SetFloat("horizontalInput", playerDirection.x);
            player.GetComponent<Animator>().SetFloat("verticalInput", playerDirection.y);
            yield return CoroutineUtilities.MoveObjectWithSpeed(player.transform, player.transform.position, player.transform.position + playerDirection, playerController.speed);

            pushableBlockToReset.GetComponent<PushableBlock>().reset();
        }
        playerController.active = true;


        yield return null;
    }
}

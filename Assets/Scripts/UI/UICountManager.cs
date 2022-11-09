using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UICountManager : MonoBehaviour
{
    public enum InventoryType
    {
        Rufee,
        Key,
        Bomb,
    }

    public InventoryType inv;
    public GameObject numPanel1;
    public GameObject numPanel2;

    private Inventory inventory;
    private PlayerController playerController;
    private Image numImage1;
    private Image numImage2;
    private Sprite nullSprite;

    private int count = -1;

    void Start()
    {
        GameObject go = GameObject.FindWithTag("Player");
        playerController = go.GetComponent<PlayerController>();
        inventory = go.GetComponent<Inventory>();
        numImage1 = numPanel1.GetComponent<Image>();
        numImage2 = numPanel2.GetComponent<Image>();
        nullSprite = getSprite("null");
    }

    void Update()
    {
        int newCount = -1;
        if (playerController.cheatMode)
        {
            newCount = 99;
        }
        else if (inv == InventoryType.Rufee)
        {
            newCount = inventory.GetRufeeCount();
        }
        else if (inv == InventoryType.Key)
        {
            newCount = inventory.GetKeyCount();
        }
        else if (inv == InventoryType.Bomb)
        {
            newCount = inventory.GetBombCount();
        }
        if (newCount != count)
        {
            count = newCount;
            updatePanel();
        }
    }

    private void updatePanel()
    {
        Debug.Log("update panel");
        if (count >= 10)
        {
            Debug.Log("sprite " + count / 10);
            numImage1.sprite = getSprite((count / 10).ToString());
            Debug.Log("sprite " + count % 10);
            numImage2.sprite = getSprite((count % 10).ToString());
        }
        else
        {
            Debug.Log("sprite " + count);
            numImage1.sprite = getSprite(count.ToString());
            numImage2.sprite = nullSprite;
        }
    }

    private Sprite getSprite(string s)
    {
        return GameAssets.GetFontSprite(s);
    }
}

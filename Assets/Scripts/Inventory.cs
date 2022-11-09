using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private PlayerController player;
    private int rufeeCount = 0;
    private int keyCount = 0;
    private int bombCount = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void AddRufee(int num)
    {
        rufeeCount = Mathf.Min(rufeeCount + num, 999);
    }

    public bool useRufee(int num)
    {
        if (player.cheatMode)
        {
            return true;
        }
        if (rufeeCount < num)
        {
            return false;
        }
        rufeeCount -= num;
        return true;
    }

    public int GetRufeeCount()
    {
        return rufeeCount;
    }

    public void AddKey(int num)
    {
        keyCount = Mathf.Min(keyCount + num, 999);
    }

    public bool useKey(int num)
    {
        if (player.cheatMode)
        {
            return true;
        }
        if (keyCount < num)
        {
            return false;
        }
        keyCount -= num;
        return true;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    public void AddBomb(int num)
    {
        bombCount = Mathf.Min(bombCount + num, 999);
    }

    public bool useBomb(int num)
    {
        if (player.cheatMode)
        {
            return true;
        }
        if (bombCount < num)
        {
            return false;
        }
        bombCount -= num;
        return true;
    }

    public int GetBombCount()
    {
        return bombCount;
    }
}

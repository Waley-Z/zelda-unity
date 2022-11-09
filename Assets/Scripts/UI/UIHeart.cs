using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    public int heartNum;

    private Image image;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    private PlayerController playerController;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        fullHeartSprite = getSprite("full_heart");
        halfHeartSprite = getSprite("half_heart");
        emptyHeartSprite = getSprite("empty_heart");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        Health h = GameObject.FindWithTag("Player").GetComponent<Health>();
        float health =  playerController.cheatMode ? h.maxHealth : h.getHealth();
        if (h.maxHealth < heartNum)
        {
            image.color = new Color(0, 0, 0, 0);
            return;
        }
        image.color = new Color(1, 1, 1, 1);
        if (health >= heartNum)
        {
            image.sprite = fullHeartSprite;
        }
        else if (health + 0.7 >= heartNum)
        {
            image.sprite = halfHeartSprite;
        } else
        {
            image.sprite = emptyHeartSprite;
        }
    }

    private Sprite getSprite(string s)
    {
        return GameAssets.GetSprite(s);
    }
}

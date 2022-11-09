// Script for having a typewriter effect for UI
// Prepared by Nick Hwang (https://www.youtube.com/nickhwang)
// Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
// Copy Paste from page into Inpector

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Typewriter : MonoBehaviour
{
    TMP_Text _tmpProText;
    public string[] texts = new string[2];

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.01f;
    public GameObject fontsHolder;
    public float interval = 1.2f;
    public float verticalInterval = 0.5f;
    private Vector3[] leftMostPosition = new Vector3[2];
    private List<GameObject> l = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            leftMostPosition[i] = new Vector3(transform.position.x - interval * texts[i].Length / 2, transform.position.y, transform.position.z);
        }
    }

    public void reset()
    {
        foreach(GameObject go in l)
        {
            Destroy(go);
        }
        l.Clear();
    }

    public IEnumerator TypeWriterTMP()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        for (int j = 0; j < texts.Length; j++)
        {
            for (int i = 0; i < texts[j].Length; i++)
            {
                GameObject g = Instantiate(fontsHolder, leftMostPosition[j] + new Vector3(interval*(i+0.5f), verticalInterval*(texts.Length/2-j), 0) , Quaternion.identity);
                SoundManager.PlaySound(SoundManager.Sound.Typewriter);
                if (texts[j][i] == ' ')
                {
                    g.GetComponent<SpriteRenderer>().color = Color.black;
                }
                else if (texts[j][i] == '.')
                {
                    g.GetComponent<SpriteRenderer>().sprite = GameAssets.GetFontSprite("period");
                }
                else
                {
                    g.GetComponent<SpriteRenderer>().sprite = GameAssets.GetFontSprite(texts[j][i].ToString());
                }
                l.Add(g);

                yield return new WaitForSeconds(timeBtwChars);
            }
        }
    }
}
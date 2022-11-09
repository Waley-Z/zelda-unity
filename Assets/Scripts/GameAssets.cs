using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


            Sprite[] allFonts = Resources.LoadAll<Sprite>("Zelda/Fonts");

            foreach (var s in allFonts)
            {
                fontList.Add(new Font { s = s.name, sprite = s });
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SoundAudioClip[] soundAudioClipArray;
    public List<Font> fontList;
    public Sprite[] spriteArray;
    public GameObject[] prefabArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class Font
    {
        public string s;
        public Sprite sprite;
    }
    public static Sprite GetFontSprite(string s)
    {
        foreach (Font font in Instance.fontList)
        {
            if ("Fonts_" + s == font.s)
            {
                return font.sprite;
            }
        }
        Debug.LogError("Sprite not found");
        return null;
    }
    public static Sprite GetSprite(string s)
    {
        foreach (Sprite sprite in Instance.spriteArray)
        {
            if (s == sprite.name)
            {
                return sprite;
            }
        }
        Debug.LogError("Sprite not found");
        return null;
    }

    public static GameObject GetPrefab(string s)
    {
        foreach (GameObject go in Instance.prefabArray)
        {
            if (s == go.name)
            {
                return go;
            }
        }
        Debug.LogError("Prefab not found");
        return null;
    }
}

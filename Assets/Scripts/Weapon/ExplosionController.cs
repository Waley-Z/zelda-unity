using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionController : MonoBehaviour
{
    public float explosionDuration = 0.7f;
    
    private Image blackPanel;
    private Color transparent = new Color(0f, 0f, 0f, 0f);
    private Color gray = new Color(1f, 1f, 1f, 0.7f);

    void Start()
    {
        blackPanel = GameObject.Find("Black Panel Mask").GetComponent<Image>();
        StartCoroutine(StartExplosion());
    }

    private IEnumerator StartExplosion()
    {
        StartCoroutine(FlashUI());
        SoundManager.PlaySound(SoundManager.Sound.Bomb);
        yield return new WaitForSeconds(explosionDuration / 2);
        GetComponent<Animator>().SetBool("transit", true);
        yield return new WaitForSeconds(explosionDuration / 2);
        Destroy(gameObject);
    }

    private IEnumerator FlashUI()
    {
        blackPanel.color = gray;
        yield return new WaitForSeconds(explosionDuration / 8);
        blackPanel.color = transparent;
        yield return new WaitForSeconds(explosionDuration / 8);
        blackPanel.color = gray;
        yield return new WaitForSeconds(explosionDuration / 8);
        blackPanel.color = transparent;
    }
}

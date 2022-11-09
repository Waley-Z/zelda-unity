using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    // public int maxHealth;
    public float maxHealth = 3f;
    public float flashInterval = 0.2f;
    public float flashTime = 1f;
    public float immuneTime = 2f;
    public float immunTimeCountDown = 0f;

    private bool isPlayer;
    public float health;
    public bool immune = false;
    private KnockBack knockBackComponent;
    private SpriteRenderer spriteRenderer;
    private float time = 0;
    private float flash = 0;
    private PlayerController playerController;
    private float originalHealth = 0;


    void Start()
    {
        isPlayer = CompareTag("Player");
        playerController = GetComponent<PlayerController>();
        health = maxHealth;
        knockBackComponent = GetComponent<KnockBack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        immunTimeCountDown -= Time.deltaTime;
        if (isPlayer && health <= 1 && health > 0)
        {
            SoundManager.PlaySound(SoundManager.Sound.LowHp);
        }
        if ((isPlayer && (playerController.cheatMode || GetComponent<Health>().health == 1)) || flash > 0)
        {
            if (time <= 0)
            {
                if (spriteRenderer.color == Color.white)
                {
                    spriteRenderer.color = Color.red;
                }
                else
                {
                    spriteRenderer.color = Color.white;
                }
                time = flashInterval;
            }
            else
            {
                time -= Time.deltaTime;
            }
            if (!(isPlayer && playerController.cheatMode))
            {
                flash -= Time.deltaTime;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public float getHealth()
    {
        return health;
    }


    public bool dead()
    {
        return health <= 0;
    }

    public void takeDamage(float n, Vector3 damageDirection, bool hasKnockBack = true)
    {
        // take damage, set immune, and knock back if player
        flash = flashTime;
        if (!immune)
        {
            health -= n;
            if (dead())
            {
                if (isPlayer)
                {
                    StartCoroutine(playerDead());
                    return;
                }
                else // enemy
                {
                    Instantiate(GameAssets.GetPrefab("death"), transform.position, Quaternion.identity);
                    if (GetComponent<WallMasterMovement>() != null)
                    {
                        GetComponent<WallMasterMovement>().triggerNext();
                    }
                    Destroy(gameObject);
                }
            }
            else if (hasKnockBack && knockBackComponent != null)
            {
                Vector3[] dir = Utils.getDirectionFromCoordinates(damageDirection - transform.position, Utils.Direction.FOUR_DIRECTIONS);
                Vector3 knockBackDirection = -dir[0];
                knockBackComponent.Knock(knockBackDirection);
                Debug.Log("knock");
            }
            if (isPlayer)
            {
                Debug.Log("play sound playerhurt");
                SoundManager.PlaySound(SoundManager.Sound.PlayerHurt);
            }
            setImmune(immuneTime);
        }
    }

    IEnumerator playerDead()
    {
        setImmune(100);
        SoundManager.pauseBGM();
        SoundManager.PlaySound(SoundManager.Sound.GameOver);
        GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(playerController.freezeControl(5));
        yield return new WaitForSeconds(5);

        bool isCustomLevel = GetComponent<PlayerController>().isCustomLevel;
        string scene = isCustomLevel ? "Custom" : "Game";
        SceneManager.LoadScene(scene);
        SoundManager.playBGM(true);

    }

    public void restoreHealth(float n)
    {
        health = Mathf.Min(health + n, maxHealth);
    }

    public void setImmune(float time)
    {
        // set immune for given amount of time
        Debug.Log("immune");
        immune = true;
        immunTimeCountDown = immuneTime;
        StartCoroutine(setImmuneCoroutine(time));
    }

    private IEnumerator setImmuneCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        immune = false;
    }

    public bool fullHealth()
    {
        return health == maxHealth;
    }
}

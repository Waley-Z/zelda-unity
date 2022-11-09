using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public enum AltWeapon
    {
        Bow,
        Bomb,
        Boomerang,
        FireBow
    }

    public float speed = 10f;
    public bool active = true;
    public Vector3 faceDirection = Utils.Direction.NONE;
    public bool cheatMode = false;
    public Sprite bowSprite;
    public Sprite bombSprite;
    public Sprite boomerangSprite;
    public Sprite fireBowSprite;
    public bool hasBow = true;
    public bool hasBomb = true;
    public bool hasBoomerang = true;
    public bool hasFireBow = true;
    public GameObject bowPrefab;
    public Vector3 originalPosition;
    public RoomInitialize currentRoom;
    public RoomInitialize firstRoom;
    public bool isCustomLevel = false;

    private Rigidbody rb;
    public Animator animator;
    private Health health;
    private Weapon standardWeapon;
    private Weapon altWeapon = null;
    public List<Weapon> altWeaponList;
    public List<Sprite> altWeaponSpriteList;
    private Inventory inventory;
    private int currentAltWeaponIndex = 0;
    private Collector collector;
    private GridMovement gm;
    private float winCheerTime = 10;
    private int[] resolution = new int[2] { 1024, 960 };

    void Awake()
    {
        originalPosition = transform.position;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        //Screen.SetResolution(1024, 960, FullScreenMode.MaximizedWindow);
        setResolution();
        currentRoom = firstRoom;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        inventory = GetComponent<Inventory>();
        standardWeapon = GetComponent<Sword>();

        if (hasBow)
        {
            addAltWeapon(AltWeapon.Bow);
        }
        if (hasBomb)
        {
            addAltWeapon(AltWeapon.Bomb);
        }
        if (hasBoomerang)
        {
            addAltWeapon(AltWeapon.Boomerang);
        }
        if (hasFireBow)
        {
            addAltWeapon(AltWeapon.FireBow);
        }
        if (altWeaponList.Count > 0)
        {
            altWeapon = altWeaponList[0];
        }
        updateAltWeaponUI();

        collector = GetComponent<Collector>();
        gm = GetComponent<GridMovement>();
    }

    private void setResolution()
    {
        Screen.SetResolution(resolution[0], resolution[1], false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            if (resolution[0] <= 2048)
            {
                resolution[0] += 96;
                resolution[1] += 90;
                setResolution();
            }
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (resolution[0] >= 512)
            {
                resolution[0] -= 96;
                resolution[1] -= 90;
                setResolution();
            }
        }
        // determine which direction the player is facing, via the animation state
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("run_right"))
        {
            faceDirection = Utils.Direction.RIGHT;
        }
        else if (state.IsName("run_left"))
        {
            faceDirection = Utils.Direction.LEFT;
        }
        else if (state.IsName("run_up"))
        {
            faceDirection = Utils.Direction.UP;
        }
        else if (state.IsName("run_down"))
        {
            faceDirection = Utils.Direction.DOWN;
        }

        // cheat mode
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("cheat code");
            if (!cheatMode)
            {
                cheatMode = true;
            }
            else
            {
                cheatMode = false;
            }
        }

        if (active)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                standardWeapon.attack(faceDirection);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (altWeapon != null)
                {
                    altWeapon.attack(faceDirection);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (altWeaponList.Count > 0)
                {
                    currentAltWeaponIndex = (currentAltWeaponIndex + 1) % altWeaponList.Count;
                    altWeapon = altWeaponList[currentAltWeaponIndex];
                    updateAltWeaponUI();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                string scene = isCustomLevel ? "Game" : "Custom";
                SceneManager.LoadScene(scene);
                SoundManager.playBGM(true);
            }
            else
            {
                movePlayerByInput();
            }
        }
    }

    private void updateAltWeaponUI()
    {
        if (altWeaponList.Count > 0)
        {
            GameObject.Find("Weapon 2 Image").GetComponent<Image>().sprite = altWeaponSpriteList[currentAltWeaponIndex];
        }
    }

    public void addAltWeapon(AltWeapon weapon, bool ifCheer = false, float cheerTime = 0.0f)
    {
        Debug.Log("add alt weapon");
        switch (weapon)
        {
            case AltWeapon.Bow:
                altWeaponList.Add(GetComponent<Bow>());
                altWeaponSpriteList.Add(bowSprite);
                break;
            case AltWeapon.Boomerang:
                altWeaponList.Add(GetComponent<Boomerang>());
                altWeaponSpriteList.Add(boomerangSprite);
                break;
            case AltWeapon.Bomb:
                altWeaponList.Add(GetComponent<Bomb>());
                altWeaponSpriteList.Add(bombSprite);
                break;
            case AltWeapon.FireBow:
                altWeaponList.Add(GetComponent<FireBow>());
                altWeaponSpriteList.Add(fireBowSprite);
                break;
            default:
                Debug.LogError("weapon does not exist");
                break;
        }
        if (altWeaponList.Count == 1)
        {
            altWeapon = altWeaponList[0];
            updateAltWeaponUI();
        }
        if (ifCheer)
        {
            StartCoroutine(WeaponCheer(cheerTime));
        }
    }

    private IEnumerator WeaponCheer(float cheerTime)
    {
        Debug.Log("Cheer");
        Vector3 bowPosition = transform.position;
        bowPosition.x -= 0.344f;
        bowPosition.y += 0.9375f;

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        animator.SetBool("cheer", true);
        active = false;
        GameObject bow = Instantiate(bowPrefab, bowPosition, Quaternion.identity);

        yield return new WaitForSeconds(cheerTime);

        Destroy(bow);
        active = true;
        animator.SetBool("cheer", false);
        rb.isKinematic = false;
    }

    public IEnumerator WinCheer()
    {
        Debug.Log("Win");

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        animator.SetBool("win", true);
        active = false;
        SoundManager.PlaySound(SoundManager.Sound.OldManRoomEnter); // same audio
        SoundManager.pauseBGM();
        yield return new WaitForSeconds(0.5f);
        SoundManager.PlaySound(SoundManager.Sound.Win);
        yield return new WaitForSeconds(winCheerTime);

        string scene = isCustomLevel ? "Custom" : "Game";
        SceneManager.LoadScene(scene);
    }

    private void movePlayerByInput()
    {
        Vector2 currentInput = getInput();

        // input to animator
        animator.SetFloat("horizontalInput", currentInput.x);
        animator.SetFloat("verticalInput", currentInput.y);
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            animator.speed = 0.0f;
        }
        else
        {
            animator.speed = 1.0f;
        }

        // control movement of player
        rb.velocity = speed * currentInput;
    }

    private Vector2 getInput()
    {
        // transform movement input into vector
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(verticalInput) > 0.0f)
        {
            StartCoroutine(gm.MoveToClosestGrid(false));
            return new Vector2(0, verticalInput > 0 ? 1 : -1);
        }
        else if (Mathf.Abs(horizontalInput) > 0.0f)
        {
            StartCoroutine(gm.MoveToClosestGrid(true));
            return new Vector2(horizontalInput > 0 ? 1 : -1, 0);
        }
        return Vector2.zero;
    }



    public IEnumerator freezeControl(float seconds)
    {
        // disable user control for given seconds
        Debug.Log("freeze control");
        active = false;
        yield return new WaitForSeconds(seconds);
        active = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            if (inventory.useKey(1))
            {
                SoundManager.PlaySound(SoundManager.Sound.DoorOpen);
                Destroy(other.gameObject);
            }
        }
    }

    public void takeDamage(float n, Vector3 damageDirection, bool hasKnockBack = true)
    {
        // player takes damage and get knocked back
        if (!cheatMode)
        {
            Vector3 knockBackDirection = -damageDirection;
            health.takeDamage(n, damageDirection, hasKnockBack);
        }
    }

}

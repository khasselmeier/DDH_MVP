using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public delegate void OnPlayerInitialized(PlayerBehavior player);
    public static event OnPlayerInitialized PlayerInitialized; // event for initializing player

    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    public float distanceToGround = 0.1f;
    public int maxHealth = 100;
    public int currentHealth;
    public int npcHitDamage = 50;

    public LayerMask groundLayer;

    [Header("Components")]
    public Rigidbody rig;
    public MeshRenderer mr;
    public PlayerRocks rocks;
    public GameObject pickaxe;

    [Header("Pickups")]
    public int gold = 0;
    public bool hasTraded = false; // tracks if the player has traded with the NPC
    public bool canMineBaseGem = false; // player must trade once to mine these gems
    public bool canMineHighGem = false; // player must trade twice to mine these gems
    public static int collectedGems = 0;
    public int totalValueOfGems = 0; // quota collected

    [Header("Animations")]
    public Animation pickaxeAnimation;
    public AnimationClip pickaxeAttackClip;

    private bool isMoving = false; // tracks whether the player is currently moving (for the footsteps audio)

    private void Start()
    {
        currentHealth = maxHealth;

        if (pickaxe != null)
        {
            pickaxe.SetActive(false); // disable pickaxe at the start
        }

        if (pickaxeAnimation != null && pickaxeAttackClip != null)
        {
            pickaxeAnimation.AddClip(pickaxeAttackClip, "PickaxeAttack");
        }

        PlayerInitialized?.Invoke(this); // notify that player is initialized
    }

    public void Initialize()
    {
        GameUI.instance.Initialize();
        currentHealth = maxHealth; // give player max health at the start
        GameUI.instance.UpdateHealthText(currentHealth, maxHealth); // update health ui
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();

        if (Input.GetMouseButtonDown(1)) // right mouse button down (button index 1)
            rocks.TryShoot();

        if (Input.GetMouseButtonDown(0)) // left mouse button down (button index 0)
        {
            // play pickaxe attack anim
            if (pickaxeAnimation != null && pickaxeAttackClip != null)
            {
                pickaxeAnimation.Play("PickaxeAttack");
            }

            TryCollectGem();
        }
    }

    void Move()
    {
        // get the input axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // calculate a direction relative to where we're facing
        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;
        dir.y = rig.velocity.y;

        // set that as our velocity
        rig.velocity = dir;

        // Check if the player is moving
        bool currentlyMoving = x != 0 || z != 0;

        if (currentlyMoving && !isMoving)
        {
            isMoving = true;
            SoundController.instance.PlayFootstepsSound(); // start playing footsteps sound
        }
        else if (!currentlyMoving && isMoving)
        {
            isMoving = false;
            SoundController.instance.StopFootstepsSound(); // stop playing footsteps sound
        }
    }

    void TryJump()
    {
        // create a ray facing down
        Ray ray = new Ray(transform.position, Vector3.down);

        // shoot the raycast
        if (Physics.Raycast(ray, 1.0f))
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //Debug.Log("Jumping");
    }

    public void TryCollectGem()
    {
        float detectionRange = 2f; //detection range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange); //get colliders within range

        foreach (var hitCollider in hitColliders)
        {
            GameObject gem = hitCollider.gameObject;

            if (gem.CompareTag("BaseGem") && canMineBaseGem)
            {
                CollectGem(gem, "BaseGem");
                return;
            }
            else if (gem.CompareTag("HighGem") && canMineHighGem)
            {
                CollectGem(gem, "HighGem");
                return;
            }
        }
    }

    public void CollectGem(GameObject gem, string gemType)
    {
        GemPickup gemPickup = gem.GetComponent<GemPickup>();
        if (gemPickup != null)
        {
            totalValueOfGems += gemPickup.gemValue;
            collectedGems += 1;
            //Debug.Log($"Player mined a {gemType}. Total collected: {collectedGems}, Total Value: {totalValueOfGems}");

            //play mining gem SFX
            SoundController.instance.PlayMiningSound();

            // update UI
            GameUI.instance.UpdateGemsValueText(totalValueOfGems);

            //destroy the gems with a delay (to give the sfx and anim time to play)
            StartCoroutine(DestroyGemWithDelay(gem, .5f));
        }
    }

    private IEnumerator DestroyGemWithDelay(GameObject gem, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gem); //destroy gem after the delay
    }

    public bool IsNearGem()
    {
        float detectionRange = 2f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (var hitCollider in hitColliders)
        {
            GameObject gem = hitCollider.gameObject;

            // check if the player can mine base or high gems
            if (gem.CompareTag("BaseGem") && canMineBaseGem || gem.CompareTag("HighGem") && canMineHighGem)
            {
                return true;
            }
        }

        return false;
    }

    public void PerformTrade()
    {
        // display the pickaxe after the first trade
        if (pickaxe != null)
        {
            pickaxe.SetActive(true);
            //Debug.Log("Pickaxe enabled");
        }
    }

    public void ChangeAmmo(int amount)
    {
        // change ammo
        rocks.curAmmo += amount;

        // ammo doesn't exceed max
        rocks.curAmmo = Mathf.Clamp(rocks.curAmmo, 0, rocks.maxAmmo);

        // update UI
        GameUI.instance.UpdateAmmoText();
    }

    public void AddAmmo(int amount)
    {
        rocks.curAmmo = Mathf.Clamp(rocks.curAmmo + amount, 0, rocks.maxAmmo);
        //Debug.Log("Ammo added: " + amount + ". Current ammo: " + rocks.curAmmo);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        //Debug.Log("Gold added: " + amount + ". Total gold: " + gold);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GameUI.instance.UpdateHealthText(currentHealth, maxHealth); // update health UI

        // play damage sound
        SoundController.instance.PlayDamageSound();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        GameUI.instance.UpdateHealthText(currentHealth, maxHealth); // update health UI
        //Debug.Log("Player healed: " + healAmount + ". Current health: " + currentHealth);
    }

    private void Die()
    {
        //Debug.Log("Player has died.");
        GameManager.instance.LoseGame();
    }

    public void OnNpcHit()
    {
        //Debug.Log("The NPC doesn't enjoy being hit");
        TakeDamage(npcHitDamage); // player takes damage when hitting the NPC
    }
}
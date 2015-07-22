using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

    public enum EnemyType { Knight, Cavalier, Healer, Archer, Boss };

    public float maxHealth = 100f;
    public int attackDamage = 1;
    public float attackDelay = 1.0f;
    public float walkingSpeed = 3.0f;
    public float healingPower = 10f;
    public float resistenceToFire = 0.0f;
    public float resistenceToWater = 0.0f;
    public float resistenceToEarthquake = 0.0f;
    public float resistenceToScream = 0.0f;
    public EnemyType enemyType;
    public GameObject projectileAttack;
    public float projectileSpeed = 5.0f;
    public float bossThrowBackForce = 4.0f;
    public float throwBackForce = 8.0f;
    public float enemyThrowBackFromRegularAttack = 2.0f;
    public bool arrivedAtDestination = false;
    public AudioClip projectileAttackSound;
    public AudioClip attackRaiseSound;
    public AudioClip attackHitSound;
    private float shakeFromDragonAttack = 0.2f;

    private GameController gameController;
    private Animator animator;
    private float lastAttackTime = 0.0f;
    private float health;
    private Player player;
    private GameObject playerObject;

    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private SpriteRenderer healthOutline;
    private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

    private bool showHealthBar = false;
    private float lastHealthChange = 0f;
    private bool beingThrownBack = false;
    private float slowFactor = 0;
    private float slowTimeEnd = 0;
    private int continuousDamage = 0;
    private float lastContinuousDamage = 0;
    private int continuousDamageCount = 0;
    private DragonAttack.AttackType continuousDamageType;
    private CameraShake camShake;

    private bool v_isFromContinuousDamage = true;
    private bool isEnemyDying = false;
    private float deleteObjectTimeAfterDeath = 0;
    private float deathFlashDelay = 0.4f;
    private float timeToLive = 3.6f;
    private SpriteRenderer enemySpriteRenderer;
    private HealingOrbScript healingOrbScript;
    
    


    void Start()
    {

        walkingSpeed = walkingSpeed / 100;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHealth;

        healthBar = transform.Find("ui_healthDisplay/Enemy_HealthBar").GetComponent<SpriteRenderer>();
        ColorHandler(healthBar, 0.0f);

        healthOutline = transform.Find("ui_healthDisplay/Enemy_HealthOutline").GetComponent<SpriteRenderer>();
        healthOutline.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        ColorHandler(healthOutline, 0.0f);


        healthScale = healthBar.transform.localScale;
        healthBar.transform.localScale = new Vector3(healthScale.x * health / maxHealth, 1, 1);
        healthBar.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;


        animator = gameObject.GetComponent<Animator>();
        if (enemyType == EnemyType.Healer)
        {
            healingOrbScript = transform.Find("HealingOrb").GetComponent<HealingOrbScript>();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (gameController.gameEnded && isEnemyDying)
        {
            Debug.Log("Enemy is dead and the level ended");
            Destroy(gameObject);
        }
       
        if (gameController.gameEnded || isEnemyDying)
            return;

        

        if ((arrivedAtDestination) && (lastAttackTime + attackDelay <= Time.time))
        {
            if (animator != null)
                animator.SetFloat("speed", 0);
            Attack();

        }


        if ((showHealthBar == true) && (Time.time > lastHealthChange + 1.0f))
        {
            //ColorHandler(healthBar, 0.0f);
            //ColorHandler(healthOutline, 0.0f);
        }

    }

    void FixedUpdate()
    {
        if (gameController.gameEnded)
            return;

        if (isEnemyDying)
        {
            ColorHandler(healthBar, 0.0f);
            ColorHandler(healthOutline, 0.0f);
            Color c = GetComponent<SpriteRenderer>().color;
            c.a -= 0.01f;
            GetComponent<SpriteRenderer>().color = c;
            if (Time.time > deleteObjectTimeAfterDeath)
                Destroy(gameObject);
        }
        
        // If the cotinuous attack is still in affect and a second passed since the last one
        if ((continuousDamageCount > 0) && (Time.time >= lastContinuousDamage + 1.0f))
        {
            Hit(continuousDamage, continuousDamageType, v_isFromContinuousDamage);
            continuousDamageCount--;
            lastContinuousDamage = Time.time;
        }
        
        if ((!arrivedAtDestination) && (!beingThrownBack))
        {
            Vector3 newPos = transform.position;
            float currentChange = (Time.time > slowTimeEnd) ? walkingSpeed : walkingSpeed * (1.0f - slowFactor);
            newPos.x -= currentChange;
            transform.position = newPos;
            if (animator != null)
            {
                animator.SetFloat("speed", currentChange);
            }
                

        }

        if (beingThrownBack)
        {

            if (rigidbody2D.velocity.x > 0)
            {
                if (animator != null)
                {
                    animator.SetFloat("speed", -1);
                }
                
                Vector2 newVelocity = rigidbody2D.velocity;
                if (enemyType == EnemyType.Boss)
                    newVelocity.x -= (bossThrowBackForce / 50);
                else
                    newVelocity.x -= (throwBackForce / 50);
                rigidbody2D.velocity = newVelocity;
            }
            else
            {

                beingThrownBack = false;
                Vector2 newVelocity = rigidbody2D.velocity;
                newVelocity.x = 0;
                rigidbody2D.velocity = newVelocity;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
            arrivedAtDestination = true;

        if ((col.gameObject.tag == "ArcherStop") && (enemyType == EnemyType.Archer))
            arrivedAtDestination = true;

        if ((col.gameObject.tag == "HealerStop") && (enemyType == EnemyType.Healer))
            arrivedAtDestination = true;


    }

    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
            arrivedAtDestination = true;

    }


    public void Hit(int damage, DragonAttack.AttackType attackType, bool isFromContinuosDamage)
    {

        float resistenceToCurrentAttack = getResistenceToAttack(attackType);
        health -= (int)(damage * (1 - resistenceToCurrentAttack));
        lastHealthChange = Time.time;
        showHealthBar = true;

        if (!isFromContinuosDamage && (attackType == DragonAttack.AttackType.Fire || attackType == DragonAttack.AttackType.Water))
        {
            if (camShake != null)
                camShake.shakeForDragonAttack = shakeFromDragonAttack;

            beingThrownBack = true;
            arrivedAtDestination = false;
            rigidbody2D.velocity = new Vector2(enemyThrowBackFromRegularAttack, 0);
        }

        UpdateHealthBar();

        if (health <= 0f)
        {
            Death();
            return;
        }

        if (attackType == DragonAttack.AttackType.Scream)
        {
            beingThrownBack = true;
            arrivedAtDestination = false;
            if (enemyType == EnemyType.Boss)
                rigidbody2D.velocity = new Vector2(bossThrowBackForce, 0);
            else
                rigidbody2D.velocity = new Vector2(throwBackForce, 0);
        }

        


    }

    private float getResistenceToAttack(DragonAttack.AttackType attackType)
    {
        float resistenceToCurrentAttack = 0f;
        switch (attackType)
        {
            case DragonAttack.AttackType.Fire:
                resistenceToCurrentAttack = resistenceToFire;
                break;
            case DragonAttack.AttackType.Water:
                resistenceToCurrentAttack = resistenceToWater;
                break;
            case DragonAttack.AttackType.Earthquake:
                resistenceToCurrentAttack = resistenceToEarthquake;
                break;
            case DragonAttack.AttackType.Scream:
                resistenceToCurrentAttack = resistenceToScream;
                break;
        }

        return resistenceToCurrentAttack;
    }

    public void SpecialHit(int damage, DragonAttack.AttackType attackType)
    {

        if (isEnemyDying)
            return;
        
        Hit(damage, attackType, false);

        if (enemyType == EnemyType.Boss)
        {
            beingThrownBack = true;
            arrivedAtDestination = false;
            rigidbody2D.velocity = new Vector2(bossThrowBackForce, 0);

        }
    }

    public void slowEnemy(float slowFactor, float slowTime)
    {
        if (isEnemyDying)
            return;
        
        if ((slowFactor == 0) || (slowTime == 0))
            return;

        this.slowFactor = slowFactor;
        this.slowTimeEnd = Time.time + slowTime;

    }

    public void continuousDamageHit(int damage, float damageTime, DragonAttack.AttackType attackType)
    {
        if (isEnemyDying)
            return;
        
        this.continuousDamage = damage;
        this.continuousDamageCount = (int) damageTime;
        this.lastContinuousDamage = Time.time;
        this.continuousDamageType = attackType;
        
    }

    private void Death()
    {

        gameController.RemoveEnemy(gameObject);
        isEnemyDying = true;
        deleteObjectTimeAfterDeath = Time.time + timeToLive;
        gameObject.layer = LayerMask.NameToLayer("Death");
        enemySpriteRenderer.sortingLayerName = "Background";
        enemySpriteRenderer.sortingOrder = 1;

        rigidbody2D.velocity = Vector2.zero;
        arrivedAtDestination = true;
        animator.SetTrigger("isDying");
        animator.SetBool("dead", true);
        
    }



    private void Attack()
    {
        if (isEnemyDying)
            return;

        lastAttackTime = Time.time;

        if (enemyType == EnemyType.Knight || enemyType == EnemyType.Cavalier)
        {
            if (gameController.isSoundEffectsOn)
                GetComponent<AudioSource>().PlayOneShot(attackRaiseSound);
        }

        if (animator != null)
        {
            animator.SetTrigger("attacking");

        }

            

    }

    public void CloseAttackDamage()
    {
        if (player != null && !isEnemyDying)
        {
            player.Hit(attackDamage);

            if (gameController.isSoundEffectsOn)
                GetComponent<AudioSource>().PlayOneShot(attackHitSound);
        }
            
    }

    private void Heal()
    {
        if (isEnemyDying)
            return;
        
        if (gameController.nonHealerEnemies() == null)
        {
            ProjectileAttack();
            return;
        }

        healingOrbScript.ShowOrb();
        ArrayList nonHealerEnemies = gameController.nonHealerEnemies();
        foreach (GameObject enemy in nonHealerEnemies)
        {
            enemy.GetComponent<Enemy>().getHealed(healingPower);

        }
    }

    public void getHealed(float amount)
    {
        if (isEnemyDying)
            return;
        
        if (health == maxHealth)
            return;

        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        lastHealthChange = Time.time;
        showHealthBar = true;
        UpdateHealthBar();
    }

    public void ProjectileAttack()
    {

        if (isEnemyDying)
            return;
        
        // Play the projectileAttackSound
        if (gameController.isSoundEffectsOn)
            GetComponent<AudioSource>().PlayOneShot(projectileAttackSound);
        
        // We get the player position and the enemy's
        Vector2 target = player.transform.position;
        Vector2 start = transform.position;

        // We will calculate the direction vector the attack is supposed to go
        Vector2 attackDirection = new Vector2(target.x - start.x, target.y - start.y);

        // Now we normalize it and multiply by the attack speed
        attackDirection = attackDirection.normalized * projectileSpeed;

        // Now we instantiate the attack at the dragon's mouth, set it's velocity and damage
        GameObject attack = (GameObject)Instantiate(projectileAttack, start, transform.rotation);
        EnemyAttack enemyAttack = attack.GetComponent<EnemyAttack>();
        enemyAttack.attackDamage = attackDamage;
        attack.rigidbody2D.velocity = attackDirection;



    }

    public void UpdateHealthBar()
    {

        ColorHandler(healthBar, 1.0f);
        ColorHandler(healthOutline, 1.0f);

        if (healthBar != null)
        {
            // Set the health bar's colour to proportion of the way between green and red based on the player's health.
            healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health / maxHealth);

            // Set the scale of the health bar to be proportional to the player's health.
            healthBar.transform.localScale = new Vector3(healthScale.x * health / maxHealth, 1, 1);
        }
        
    }


    public void ColorHandler(SpriteRenderer spriteRenderer, float f)
    {
        if (spriteRenderer != null)
        {
            Color temporaryColorHolder = spriteRenderer.material.color;
            temporaryColorHolder.a = f;
            spriteRenderer.material.color = temporaryColorHolder;
        }
        
    }
}

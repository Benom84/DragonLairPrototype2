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
    public float resistenceToAir = 0.0f;
    public float resistenceToEarthquake = 0.0f;
    public float resistenceToScream = 0.0f;
    public EnemyType enemyType;
    public GameObject projectileAttack;
    public float projectileSpeed = 5.0f;
    public float bossThrowBackForce = 4.0f;
    public float throwBackForce = 8.0f;

    private GameController gameController;
    private Animator animator;
    private bool arrivedAtDestination = false;
    private float lastAttackTime = 0.0f;
    private float health;
    private Player player;
    private GameObject playerObject;

    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private SpriteRenderer healthOutline;
    private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

    private bool showHealthBar = false;
    private float lastHealthChange = 0f;
    private float lastWallHit = 0f;
    private bool beingThrownBack = false;

    // Use this for initialization
    void Start()
    {

        walkingSpeed = walkingSpeed / 100;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        health = maxHealth;

        healthBar = transform.Find("ui_healthDisplay/Enemy_HealthBar").GetComponent<SpriteRenderer>();
        ColorHandler(healthBar, 0.0f);

        healthOutline = transform.Find("ui_healthDisplay/Enemy_HealthOutline").GetComponent<SpriteRenderer>();
        ColorHandler(healthOutline, 0.0f);


        healthScale = healthBar.transform.localScale;
        healthBar.transform.localScale = new Vector3(healthScale.x * health / maxHealth, 1, 1);


        animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (gameController.gameEnded)
            return;

        if ((arrivedAtDestination) && (lastAttackTime + attackDelay <= Time.time))
        {
            if (animator != null)
                animator.SetFloat("speed", 0);
            Attack();

        }


        if ((showHealthBar == true) && (Time.time > lastHealthChange + 1.0f))
        {
            ColorHandler(healthBar, 0.0f);
            ColorHandler(healthOutline, 0.0f);
        }

    }

    void FixedUpdate()
    {
        if ((!arrivedAtDestination) && (!beingThrownBack))
        {
            Vector3 newPos = transform.position;
            newPos.x -= walkingSpeed;
            transform.position = newPos;
            if (animator != null)
                animator.SetFloat("speed", walkingSpeed);

        }

        if (beingThrownBack)
        {

            if (rigidbody2D.velocity.x > 0)
            {
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


    public void Hit(int damage, DragonAttack.AttackType attackType)
    {

        float resistenceToCurrentAttack = getResistenceToAttack(attackType);
        health -= (int)(damage * (1 - resistenceToCurrentAttack));
        lastHealthChange = Time.time;
        showHealthBar = true;
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

        UpdateHealthBar();


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

        Hit(damage, attackType);

        if (enemyType == EnemyType.Boss)
        {
            beingThrownBack = true;
            arrivedAtDestination = false;
            rigidbody2D.velocity = new Vector2(bossThrowBackForce, 0);

        }
    }

    public void WallHit(int damage, DragonAttack.AttackType attackType, float timeDelay)
    {

        if (Time.time < lastWallHit + timeDelay)
            return;

        lastWallHit = Time.time;

        Hit(damage, attackType);

    }

    private void Death()
    {

        gameController.RemoveEnemy(gameObject);
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetTrigger("attacking");

        if (enemyType == EnemyType.Healer)
        {
            if (gameController.nonHealerEnemies() != null)
                Heal();
            else
                ProjectileAttack();
        }

        if (enemyType == EnemyType.Archer)
            ProjectileAttack();

        if ((enemyType == EnemyType.Knight) || (enemyType == EnemyType.Cavalier))
            player.Hit(attackDamage);

    }

    private void Heal()
    {
        ArrayList nonHealerEnemies = gameController.nonHealerEnemies();
        foreach (GameObject enemy in nonHealerEnemies)
        {
            enemy.GetComponent<Enemy>().getHealed(healingPower);

        }
    }

    public void getHealed(float amount)
    {
        if (health == maxHealth)
            return;

        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        lastHealthChange = Time.time;
        showHealthBar = true;
        UpdateHealthBar();
    }

    private void ProjectileAttack()
    {

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


        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health / maxHealth);

        // Set the scale of the health bar to be proportional to the player's health.
        healthBar.transform.localScale = new Vector3(healthScale.x * health / maxHealth, 1, 1);
    }


    public void ColorHandler(SpriteRenderer spriteRenderer, float f)
    {
        Color temporaryColorHolder = spriteRenderer.material.color;
        temporaryColorHolder.a = f;
        spriteRenderer.material.color = temporaryColorHolder;
    }
}
